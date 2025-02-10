﻿using ApplicationLayer.BusinessLogic.Interfaces;
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

        public ServiceResult GetUserByValidationMethodAsync(LoginViewModel loginViewModel)
        {
            try
            {
                var result = _userAccountRepository.Query();

                if (loginViewModel.ValidationMethod == ValidationMethod.OneTimePasswordEmail)
                    result = result.Where(current => current.Email == loginViewModel.UserName);
                else if (loginViewModel.ValidationMethod == ValidationMethod.OneTimePasswordMobile)
                    result = result.Where(current => current.PhoneNumber == loginViewModel.UserName);
                else
                    result = result.Where(
                        current => current.UserName == loginViewModel.UserName
                        || current.Email == loginViewModel.UserName
                        || current.PhoneNumber == loginViewModel.UserName);

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
    }
}