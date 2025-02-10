using ApplicationLayer.ViewModels.Identity;
using DomainLayer.Entities;

namespace ApplicationLayer.BusinessLogic.Interfaces
{
    public interface IIdentityService
    {
        ServiceResult AuthenticateOneTimePassword(LoginViewModel loginViewModel, UserAccount userAccount);

        ServiceResult AuthenticateUserInformation(LoginViewModel loginViewModel, UserAccount userAccount);

        (string jwtToken, string tokenId) TokenGenerator(string userName, int userId);
    }
}