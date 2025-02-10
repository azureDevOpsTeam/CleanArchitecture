using ApplicationLayer.ViewModels.Identity;
using DomainLayer.Entities;

namespace ApplicationLayer.BusinessLogic.Interfaces
{
    public interface IIdentityService
    {
        ServiceResult AuthenticateOneTimePassword(SignInViewModel signInViewModel, UserAccount userAccount);

        ServiceResult AuthenticateUserInformation(SignInViewModel signInViewModel, UserAccount userAccount);

        (string jwtToken, string tokenId) TokenGenerator(string userName, int userId);
    }
}