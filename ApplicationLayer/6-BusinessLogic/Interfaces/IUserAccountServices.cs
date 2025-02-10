using ApplicationLayer.ViewModels.Identity;
using DomainLayer.Entities;

namespace ApplicationLayer.BusinessLogic.Interfaces
{
    public interface IUserAccountServices
    {
        Task<UserAccount> GetUserAccountByIdAsync(int accountId);

        ServiceResult GetUserByValidationMethodAsync(LoginViewModel loginViewModel);
    }
}