using ApplicationLayer.ViewModels.Identity;
using DomainLayer.Entities;

namespace ApplicationLayer.BusinessLogic.Interfaces
{
    public interface IUserAccountServices
    {
        Task<UserAccount> GetUserAccountByIdAsync(int accountId);

        ServiceResult GetUserByValidationMethodAsync(SignInViewModel signInViewModel);

        Task<ServiceResult> AddProfileAsync(UserProfile model);

        Task<ServiceResult> AddUserAccountAsync(UserAccount model);
    }
}