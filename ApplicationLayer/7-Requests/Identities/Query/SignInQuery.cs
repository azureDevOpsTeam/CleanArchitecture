using ApplicationLayer.ViewModels.Identity;
using MediatR;

namespace ApplicationLayer.Requests.Identities.Query
{
    public class SignInQuery : IRequest<HandlerResult>
    {
        public SignInViewModel InputData { get; set; }
    }
}