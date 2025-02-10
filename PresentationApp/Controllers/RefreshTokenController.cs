using ApplicationLayer.Extensions;
using ApplicationLayer.Requests.RefreshTokens.Command;
using ApplicationLayer.Requests.RefreshTokens.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PresentationApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> RefreshToken(TokenRequestQuery model)
            => await ResultHelper.GetResultAsync(_mediator, model);

        [HttpPost]
        [Route("RevokeRefreshToken")]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenCommand model)
            => await ResultHelper.GetResultAsync(_mediator, model);
    }
}