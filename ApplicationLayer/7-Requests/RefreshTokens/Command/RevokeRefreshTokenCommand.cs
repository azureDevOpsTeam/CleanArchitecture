using ApplicationLayer.ViewModels.RefreshTokens;
using MediatR;

namespace ApplicationLayer.Requests.RefreshTokens.Command
{
    public class RevokeRefreshTokenCommand : IRequest<HandlerResult>
    {
        public RevokeRefreshTokenViewModel InputData { get; set; }
    }
}