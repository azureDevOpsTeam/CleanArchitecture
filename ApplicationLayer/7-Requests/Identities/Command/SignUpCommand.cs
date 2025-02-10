using ApplicationLayer.ViewModels.Identity;
using MediatR;

namespace ApplicationLayer.Requests.Identities.Command
{
    public class SignUpCommand : IRequest<HandlerResult>
    {
        public SignUpViewModel InputData { get; set; }
    }
}
