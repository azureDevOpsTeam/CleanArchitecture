using ApplicationLayer.BusinessLogic.Services;
using ApplicationLayer.Extensions.ServiceMessages;
using ApplicationLayer.Extensions.SmartEnums;
using ApplicationLayer.ViewModels.Identity;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace CleanArchitecture.UnitTest.Services.IdentityTest
{
    public class IdentityServiceTest
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<IdentityService>> _mockLogger;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly IdentityService _identityService;

        public IdentityServiceTest()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<IdentityService>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            _identityService = new IdentityService(
                _mockConfiguration.Object,
                _mockLogger.Object,
                _mockHttpContextAccessor.Object
            );
        }

        [Fact]
        public void AuthenticateOneTimePassword_Successful()
        {
            var loginViewModel = new LoginViewModel { UserName = "testuser", SecurityCode = 123456 };
            var userAccount = new UserAccount { SecurityCode = 123456, ExpireSecurityCode = DateTime.Now.AddMinutes(5) };

            _mockConfiguration.Setup(c => c["JWT:Key"]).Returns("rZ5GvP7Qk9eA3D1jN8iR6hYtO2fW4sLmK0xU1cBnJdXpFySgEwMqCzVbH3uI5oT");

            var result = _identityService.AuthenticateOneTimePassword(loginViewModel, userAccount);

            Assert.Equal(RequestStatus.Successful, result.RequestStatus);
        }

        [Fact]
        public void AuthenticateOneTimePassword_IncorrectSecurityCode()
        {
            var loginViewModel = new LoginViewModel { UserName = "testuser", SecurityCode = 654321 };
            var userAccount = new UserAccount { SecurityCode = 123456, ExpireSecurityCode = DateTime.Now.AddMinutes(5) };

            var result = _identityService.AuthenticateOneTimePassword(loginViewModel, userAccount);

            Assert.Equal(RequestStatus.NotFound, result.RequestStatus);
            Assert.Equal(IdentityMessages.IncorrectSecurityCode, result.Message);
        }
    }
}