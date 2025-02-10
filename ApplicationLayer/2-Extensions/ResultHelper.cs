using ApplicationLayer.Common;
using ApplicationLayer.Extensions.SmartEnums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationLayer.Extensions
{
    public static class ResultHelper
    {
        public static async Task<IActionResult> GetResultAsync<T>(IMediator mediator, T mediate)
        {
            HandlerResult result = (HandlerResult)await mediator.Send(mediate);

            if (result.RequestStatus == RequestStatus.Successful || result.RequestStatus == RequestStatus.RefreshNotRequired)
                return new OkObjectResult(result);
            else if (result.RequestStatus == RequestStatus.NotFound || result.RequestStatus == RequestStatus.IncorrectUser)
                return new NotFoundObjectResult(result);
            else if (result.RequestStatus == RequestStatus.AuthenticationFailed)
                return new UnauthorizedObjectResult(result);
            else if (result.RequestStatus == RequestStatus.Duplicated || result.RequestStatus == RequestStatus.Exists)
                return new ConflictObjectResult(result);
            else
                return new BadRequestObjectResult(result);
        }
    }
}