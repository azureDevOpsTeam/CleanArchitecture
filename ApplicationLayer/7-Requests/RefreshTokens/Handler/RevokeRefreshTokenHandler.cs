using ApplicationLayer.Extensions.ServiceMessages;
using ApplicationLayer.Extensions.SmartEnums;
using ApplicationLayer.Extensions.Utilities;
using ApplicationLayer.Requests.RefreshTokens.Command;
using ApplicationLayer.ViewModels.RefreshTokens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Requests.RefreshTokens.Handler
{
    public class RevokeRefreshTokenHandler(IRefreshTokenService refreshTokenService,
                                                  IUnitOfWork unitOfWork,
                                                  ILogger<RevokeRefreshTokenViewModel> logger) : IRequestHandler<RevokeRefreshTokenCommand, HandlerResult>
    {
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<RevokeRefreshTokenViewModel> _logger = logger;

        public async Task<HandlerResult> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _refreshTokenService.RevokeRefreshTokenByUserId(request.InputData.UserId);

                if (result.RequestStatus != RequestStatus.Successful)
                {
                    return new HandlerResult<RevokeRefreshTokenViewModel>()
                    {
                        RequestStatus = result.RequestStatus,
                        Data = request.InputData,
                        Message = result.Message
                    };
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new HandlerResult<RevokeRefreshTokenViewModel>()
                {
                    RequestStatus = RequestStatus.Successful,
                    Data = request.InputData,
                    Message = CommonMessages.Successful
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, CommonMessages.Failed);
                return new HandlerResult<RevokeRefreshTokenViewModel>()
                {
                    RequestStatus = RequestStatus.Failed,
                    Data = request.InputData,
                    Message = CommonMessages.Failed
                };
            }
        }
    }
}