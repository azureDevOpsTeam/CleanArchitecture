using ApplicationLayer.BusinessLogic.Interfaces;
using ApplicationLayer.Extensions;
using ApplicationLayer.Extensions.ServiceMessages;
using ApplicationLayer.Extensions.SmartEnums;
using ApplicationLayer.Requests.Identities.Command;
using ApplicationLayer.ViewModels.Identity;
using AutoMapper;
using DomainLayer.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Requests.Identities.Handler
{
    public class SignUpHandler(IUnitOfWork unitOfWork, IUserAccountServices userAccountServices, IMapper mapper, ILogger<SignUpHandler> logger) : IRequestHandler<SignUpCommand, HandlerResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly IUserAccountServices _userAccountServices = userAccountServices;

        private readonly IMapper _mapper = mapper;

        private readonly ILogger<SignUpHandler> _logger = logger;

        public async Task<HandlerResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var userAccount = _mapper.Map<UserAccount>(request.InputData);
                var userProfile = _mapper.Map<UserProfile>(request.InputData);

                userAccount.Password = HashGenerator.GenerateSHA256HashWithSalt(request.InputData.Password, out string securityStamp);
                userAccount.SecurityStamp = securityStamp;

                var userAccountResult = await _userAccountServices.AddUserAccountAsync(userAccount);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (userAccountResult.RequestStatus != RequestStatus.Successful)
                {
                    await _unitOfWork.RollbackAsync();
                    return new HandlerResult<SignUpViewModel> { RequestStatus = userAccountResult.RequestStatus, Data = request.InputData, Message = userAccountResult.Message };
                }

                userProfile.UserAccountId = userAccount.Id;
                var profileResult = await _userAccountServices.AddProfileAsync(userProfile);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _unitOfWork.CommitAsync();
                return new HandlerResult<SignUpViewModel> { RequestStatus = userAccountResult.RequestStatus, Data = request.InputData, Message = userAccountResult.Message };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                await _unitOfWork.RollbackAsync();
                return new HandlerResult<SignUpViewModel> { RequestStatus = RequestStatus.Failed, Data = request.InputData, Message = CommonMessages.Failed };
            }
        }
    }
}