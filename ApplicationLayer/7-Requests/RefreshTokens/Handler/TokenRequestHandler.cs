using ApplicationLayer.BusinessLogic.Interfaces;
using ApplicationLayer.Extensions.ServiceMessages;
using ApplicationLayer.Extensions.SmartEnums;
using ApplicationLayer.Requests.RefreshTokens.Query;
using ApplicationLayer.ViewModels;
using DomainLayer.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Requests.RefreshTokens.Handler
{
    public class TokenRequestHandler(IIdentityService identityService,
                                          IRefreshTokenService refreshTokenService,
                                          IUserAccountServices userAccountServices,
                                          IUnitOfWork unitOfWork,
                                          ILogger<TokenRequestViewModel> logger) : IRequestHandler<TokenRequestQuery, HandlerResult>
    {
        private readonly IIdentityService _identityService = identityService;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
        private readonly IUserAccountServices _userAccountServices = userAccountServices;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<TokenRequestViewModel> _logger = logger;

        public async Task<HandlerResult> Handle(TokenRequestQuery request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _refreshTokenService.VerifyToken(request.InputData);

                if (result.RequestStatus != RequestStatus.Successful)
                {
                    await _unitOfWork.RollbackAsync();
                    return new HandlerResult<TokenRequestViewModel>()
                    {
                        RequestStatus = result.RequestStatus,
                        Data = request.InputData,
                        Message = result.Message
                    };
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var storedToken = (RefreshToken)result.Data;

                var userModel = await _userAccountServices.GetUserAccountByIdAsync(storedToken.UserAccountId);

                var token = _identityService.TokenGenerator(userModel.UserName, userModel.Id);

                var refreshToken = _refreshTokenService.RefreshTokenGenerator(userModel.Id, token.tokenId);
                refreshToken.UserFullName = storedToken.UserFullName;

                var refreshTokenResult = _refreshTokenService.AddRefreshToken(refreshToken);

                if (refreshTokenResult.RequestStatus != RequestStatus.Successful)
                {
                    await _unitOfWork.RollbackAsync();
                    return new HandlerResult<TokenRequestViewModel>
                    {
                        RequestStatus = refreshTokenResult.RequestStatus,
                        Data = null,
                        Message = refreshTokenResult.Message
                    };
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitAsync();

                var authorizeResultViewModel = new AuthorizeResultViewModel()
                {
                    AccessTokens = token.jwtToken,
                    RefreshToken = refreshToken.Token,
                    UserFullName = storedToken.UserFullName
                };

                return new HandlerResult<TokenRequestViewModel>
                {
                    RequestStatus = RequestStatus.Successful,
                    Data = authorizeResultViewModel,
                    Message = CommonMessages.Successful
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(message: ex.Message, CommonMessages.Failed);
                return new HandlerResult<TokenRequestViewModel>()
                {
                    RequestStatus = RequestStatus.Failed,
                    Data = request.InputData,
                    Message = CommonMessages.Failed
                };
            }
        }
    }
}