using ApplicationLayer.BusinessLogic.Interfaces;
using ApplicationLayer.Extensions;
using ApplicationLayer.Extensions.ServiceMessages;
using ApplicationLayer.Extensions.SmartEnums;
using ApplicationLayer.ViewModels.Identity;
using DomainLayer.Common.Attributes;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApplicationLayer.BusinessLogic.Services
{
    [InjectAsScoped]
    public class IdentityService(IConfiguration iConfiguration,
                                 ILogger<IdentityService> logger,
                                 IHttpContextAccessor httpContextAccessor) : IIdentityService
    {
        private readonly IConfiguration _iConfiguration = iConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<IdentityService> _logger = logger;

        public ServiceResult AuthenticateOneTimePassword(SignInViewModel loginViewModel, UserAccount userAccount)
        {
            if (loginViewModel.SecurityCode != userAccount.SecurityCode || DateTime.Now > userAccount.ExpireSecurityCode)
            {
                return new ServiceResult
                {
                    RequestStatus = RequestStatus.NotFound,
                    Data = loginViewModel,
                    Message = IdentityMessages.IncorrectSecurityCode
                };
            }

            AuthorizeResultViewModel result = new()
            {
                UserFullName = loginViewModel.UserName
            };

            return new ServiceResult
            {
                RequestStatus = RequestStatus.Successful,
                Data = result,
                Message = CommonMessages.Successful
            };
        }

        public ServiceResult AuthenticateUserInformation(SignInViewModel loginViewModel, UserAccount userAccount)
        {
            var passwordIsValid = HashGenerator.VerifyPassword(loginViewModel.Password, userAccount.Password, userAccount.SecurityStamp);
            if (!passwordIsValid)
                return new ServiceResult { RequestStatus = RequestStatus.Failed, Message = IdentityMessages.IncorrectPassword };
            else
            {
                AuthorizeResultViewModel result = new();
                //var getAccessToken = TokenGenerator(loginViewModel.UserName, userAccount.Id);
                //result.AccessTokens = getAccessToken;
                result.UserFullName = loginViewModel.UserName;

                return new ServiceResult { RequestStatus = RequestStatus.Successful, Data = result, Message = CommonMessages.Successful };
            }
        }

        public (string jwtToken, string tokenId) TokenGenerator(string userName, int userId)
        {
            if (_iConfiguration == null) throw new ArgumentNullException(nameof(_iConfiguration));
            if (_httpContextAccessor == null) throw new ArgumentNullException(nameof(_httpContextAccessor));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_iConfiguration["JWT:Key"]);

            if (tokenKey.Length < 32) throw new ArgumentException("JWT key must be at least 256 bits (32 bytes) long.");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new(ClaimTypes.Name, userName),
                    new(ClaimTypes.NameIdentifier, userId.ToString()),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                ]),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_iConfiguration.GetSection("JWT:JwtTokenExpirationTimeInMinutes").Value)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = tokenHandler.WriteToken(token);

            return (jwtToken, token.Id);
        }
    }
}