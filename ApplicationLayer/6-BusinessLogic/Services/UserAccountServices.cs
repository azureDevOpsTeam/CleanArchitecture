using ApplicationLayer.BusinessLogic.Interfaces;
using ApplicationLayer.Extensions.ServiceMessages;
using ApplicationLayer.Extensions.SmartEnums;
using ApplicationLayer.ViewModels.Identity;
using DomainLayer.Common.Attributes;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.BusinessLogic.Services
{
    [InjectAsScoped]
    public class UserAccountServices(IRepository<UserAccount> userAccountRepository, IRepository<UserProfile> userProfileRepository, ILogger<UserAccountServices> logger) : IUserAccountServices
    {
        private readonly IRepository<UserAccount> _userAccountRepository = userAccountRepository;
        private readonly IRepository<UserProfile> _userProfileRepository = userProfileRepository;
        private readonly ILogger<UserAccountServices> _logger = logger;

        public async Task<UserAccount> GetUserAccountByIdAsync(int accountId)
            => await Task.Run(() => _userAccountRepository.GetDbSet().FirstOrDefaultAsync(row => row.Id == accountId));

        public ServiceResult GetUserByValidationMethodAsync(SignInViewModel signInViewModel)
        {
            try
            {
                var result = _userAccountRepository.Query();

                if (signInViewModel.ValidationMethod == ValidationMethod.OneTimePasswordEmail)
                    result = result.Where(current => current.Email == signInViewModel.UserName);
                else if (signInViewModel.ValidationMethod == ValidationMethod.OneTimePasswordMobile)
                    result = result.Where(current => current.PhoneNumber == signInViewModel.UserName);
                else
                    result = result.Where(
                        current => current.UserName == signInViewModel.UserName
                        || current.Email == signInViewModel.UserName
                        || current.PhoneNumber == signInViewModel.UserName);

                if (!result?.Any() ?? true)
                {
                    return new ServiceResult
                    {
                        RequestStatus = RequestStatus.NotFound,
                        Data = null,
                        Message = CommonMessages.NotFound
                    };
                }

                return new ServiceResult
                {
                    RequestStatus = RequestStatus.Successful,
                    Data = result.FirstOrDefault(),
                    Message = CommonMessages.Successful
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(message: exception.Message, CommonMessages.Failed);

                return new ServiceResult
                {
                    RequestStatus = RequestStatus.Failed,
                    Data = null,
                    Message = CommonMessages.Failed
                };
            }
        }

        public async Task<ServiceResult> AddUserAccountAsync(UserAccount model)
        {
            try
            {
                await _userAccountRepository.AddAsync(model);
                return new ServiceResult { RequestStatus = RequestStatus.Successful, Data = model, Message = CommonMessages.Successful };
            }
            catch (Exception exception)
            {
                _logger.LogError(message: exception.Message, CommonMessages.Failed);
                return new ServiceResult { RequestStatus = RequestStatus.Failed, Data = model, Message = CommonMessages.Failed };
            }
        }

        public async Task<ServiceResult> AddProfileAsync(UserProfile model)
        {
            try
            {
                model.IsActive = true;
                await _userProfileRepository.AddAsync(model);
                return new ServiceResult { RequestStatus = RequestStatus.Successful, Data = model, Message = CommonMessages.Successful };
            }
            catch (Exception exception)
            {
                _logger.LogError(message: exception.Message, CommonMessages.Failed);
                return new ServiceResult { RequestStatus = RequestStatus.Failed, Data = model, Message = CommonMessages.Failed };
            }
        }
    }
}