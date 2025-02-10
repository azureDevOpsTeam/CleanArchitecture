using ApplicationLayer.BusinessLogic.Interfaces;
using ApplicationLayer.Extensions.SmartEnums;
using ApplicationLayer.Requests.Identities.Query;
using ApplicationLayer.ViewModels.Identity;
using DomainLayer.Entities;
using MediatR;

namespace ApplicationLayer.Requests.Identities.Handler
{
    public class LoginHandler(IIdentityService identityService,
                                   IRefreshTokenService refreshTokenService,
                                   IUserAccountServices userAccountServices,
                                   IUnitOfWork unitOfWork) : IRequestHandler<LoginQuery, HandlerResult>
    {
        private readonly IIdentityService _identityService = identityService;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
        private readonly IUserAccountServices _userAccountServices = userAccountServices;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<HandlerResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var userResult = _userAccountServices.GetUserByValidationMethodAsync(request.InputData);

            var userModel = (UserAccount)userResult.Data;

            var serviceResult = new ServiceResult();

            if (userResult.RequestStatus == RequestStatus.Successful)
            {
                //Login by One Time Password In Email
                if (request.InputData.ValidationMethod == ValidationMethod.OneTimePasswordEmail || request.InputData.ValidationMethod == ValidationMethod.OneTimePasswordMobile)
                    serviceResult = _identityService.AuthenticateOneTimePassword(request.InputData, (UserAccount)userResult.Data);
                else
                    serviceResult = _identityService.AuthenticateUserInformation(request.InputData, userModel);
            }
            else
            {
                return new HandlerResult<LoginViewModel>
                {
                    RequestStatus = userResult.RequestStatus,
                    Data = null,
                    Message = userResult.Message
                };
            }

            if (serviceResult.RequestStatus != RequestStatus.Successful)
            {
                return new HandlerResult<LoginViewModel>
                {
                    RequestStatus = serviceResult.RequestStatus,
                    Data = null,
                    Message = serviceResult.Message
                };
            }

            AuthorizeResultViewModel authorizeResultViewModel = (AuthorizeResultViewModel)serviceResult.Data;

            var token = _identityService.TokenGenerator(userModel.UserName, userModel.Id);
            var refreshToken = _refreshTokenService.RefreshTokenGenerator(userModel.Id, token.tokenId);
            refreshToken.UserFullName = authorizeResultViewModel.UserFullName;
            var refreshTokenResult = _refreshTokenService.AddRefreshToken(refreshToken);

            if (refreshTokenResult.RequestStatus != RequestStatus.Successful)
                return new HandlerResult<LoginViewModel>
                {
                    RequestStatus = refreshTokenResult.RequestStatus,
                    Data = null,
                    Message = refreshTokenResult.Message
                };

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            authorizeResultViewModel.AccessTokens = token.jwtToken;
            authorizeResultViewModel.RefreshToken = refreshToken.Token;

            return new HandlerResult<LoginViewModel>
            {
                RequestStatus = serviceResult.RequestStatus,
                Data = authorizeResultViewModel,
                Message = serviceResult.Message
            };
        }
    }
}