using ApplicationLayer.Extensions;
using ApplicationLayer.Requests.Identities.Command;
using ApplicationLayer.Requests.Identities.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignInAsync(SignInQuery model)
            => await ResultHelper.GetResultAsync(_mediator, model);

        [HttpPost]
        [Route("SignUpCommand")]
        public async Task<IActionResult> CreateUserAccountAsync(SignUpCommand model)
            => await ResultHelper.GetResultAsync(_mediator, model);
    }
}