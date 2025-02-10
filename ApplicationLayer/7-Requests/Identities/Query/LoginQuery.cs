using ApplicationLayer.ViewModels.Identity;
using MediatR;

namespace ApplicationLayer.Requests.Identities.Query
{
    public class LoginQuery : IRequest<HandlerResult>
    {
        public LoginViewModel InputData { get; set; }
    }
}