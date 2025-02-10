using ApplicationLayer.Extensions.ServiceMessages;
using ApplicationLayer.Extensions.SmartEnums;
using ApplicationLayer.ViewModels;
using DomainLayer.Common.Attributes;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApplicationLayer.Extensions.Utilities
{
    [InjectAsScoped]
    public class RefreshTokenService(IConfiguration iConfiguration,
                                     TokenValidationParameters tokenValidationParameters,
                                     ILogger<RefreshTokenService> logger,
                                     IRepository<RefreshToken> _refreshTokenRepository) : IRefreshTokenService
    {
        private readonly IConfiguration _iConfiguration = iConfiguration;
        private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;
        private readonly ILogger<RefreshTokenService> _logger = logger;
        private readonly IRepository<RefreshToken> _refreshTokenRepository = _refreshTokenRepository;

        public ServiceResult AddRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                _refreshTokenRepository.Add(refreshToken);

                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Successful,
                    Data = refreshToken,
                    Message = CommonMessages.Successful,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, CommonMessages.Failed);
                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Failed,
                    Data = null,
                    Message = CommonMessages.Failed
                };
            }
        }

        public ServiceResult UpdateRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                _refreshTokenRepository.Update(refreshToken);
                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Successful,
                    Data = true,
                    Message = CommonMessages.Successful,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, CommonMessages.Failed);
                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Failed,
                    Data = null,
                    Message = CommonMessages.Failed
                };
            }
        }

        public RefreshToken RefreshTokenGenerator(int userId, string tokenId)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz_";
            var randomString = new string(Enumerable.Repeat(chars, 23).Select(s => s[random.Next(s.Length)]).ToArray());

            var refreshToken = new RefreshToken()
            {
                JwtId = tokenId,
                Token = randomString,
                ExpiryDate = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_iConfiguration.GetSection("JWT:RefreshTokenExpirationTimeInMinutes").Value)),
                IsRevoked = false,
                IsUsed = false,
                UserAccountId = userId,
            };
            return refreshToken;
        }

        public async Task<ServiceResult> VerifyToken(TokenRequestViewModel tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                //For Test
                _tokenValidationParameters.ValidateLifetime = false;

                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.AccessTokens, _tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return new ServiceResult()
                        {
                            RequestStatus = RequestStatus.Failed,
                            Data = null,
                            Message = CommonMessages.Failed
                        };
                    }
                }


                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = TimeHelper.UnixTimeStampToDateTime(utcExpiryDate);

                var storedToken = await _refreshTokenRepository.GetDbSet()
                    .FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (expiryDate > DateTime.UtcNow && !storedToken.IsRevoked)
                {
                    return new ServiceResult()
                    {
                        RequestStatus = RequestStatus.RefreshNotRequired,
                        Data = null,
                        Message = CommonMessages.RefreshNotRequired
                    };
                }


                if (storedToken is null)
                {
                    return new ServiceResult()
                    {
                        RequestStatus = RequestStatus.InvalidToken,
                        Data = null,
                        Message = CommonMessages.InvalidToken
                    };
                }

                if (storedToken.IsUsed)
                {
                    return new ServiceResult()
                    {
                        RequestStatus = RequestStatus.InvalidToken,
                        Data = null,
                        Message = CommonMessages.InvalidToken
                    };
                }

                if (storedToken.IsRevoked)
                {
                    return new ServiceResult()
                    {
                        RequestStatus = RequestStatus.InvalidToken,
                        Data = null,
                        Message = CommonMessages.InvalidToken
                    };
                }

                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (jti is null || storedToken.JwtId != jti)
                {
                    return new ServiceResult()
                    {
                        RequestStatus = RequestStatus.InvalidToken,
                        Data = null,
                        Message = CommonMessages.InvalidToken
                    };
                }

                if (storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new ServiceResult()
                    {
                        RequestStatus = RequestStatus.ExpiredToken,
                        Data = null,
                        Message = CommonMessages.ExpiredToken
                    };
                }

                storedToken.IsUsed = true;
                UpdateRefreshToken(storedToken);

                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Successful,
                    Data = storedToken,
                    Message = CommonMessages.Successful
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, CommonMessages.Failed);
                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Failed,
                    Data = null,
                    Message = CommonMessages.Failed
                };
            }
        }

        public ServiceResult RemoveExpiredTokensFromDatabase()
        {
            try
            {
                var expiredTokens = _refreshTokenRepository.GetDbSet()
                    .Where(t => t.ExpiryDate < DateTime.UtcNow || t.IsUsed);

                _refreshTokenRepository.DeleteRangeFromDatabase(expiredTokens);

                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Successful,
                    Data = true,
                    Message = CommonMessages.Successful,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, CommonMessages.Failed);
                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Failed,
                    Data = null,
                    Message = CommonMessages.Failed
                };
            }
        }

        public async Task<ServiceResult> RevokeRefreshTokenByUserId(int userId)
        {
            try
            {
                var refreshTokens = await _refreshTokenRepository
                    .GetDbSet()
                    .Where(r => r.UserAccountId == userId)
                    .ToListAsync();

                if (!refreshTokens?.Any() ?? true)
                {
                    return new ServiceResult()
                    {
                        RequestStatus = RequestStatus.NotFound,
                        Data = false,
                        Message = CommonMessages.NotFound,
                    };
                }

                refreshTokens.ForEach(token =>
                {
                    token.IsRevoked = true;
                    UpdateRefreshToken(token);
                });

                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Successful,
                    Data = true,
                    Message = CommonMessages.Successful,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, CommonMessages.Failed);
                return new ServiceResult()
                {
                    RequestStatus = RequestStatus.Failed,
                    Data = null,
                    Message = CommonMessages.Failed
                };
            }
        }
    }
}