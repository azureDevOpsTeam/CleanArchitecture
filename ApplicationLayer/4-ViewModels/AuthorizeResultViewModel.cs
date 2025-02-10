using ApplicationLayer.Extensions.SmartEnums;

namespace ApplicationLayer.ViewModels
{
    public class AuthorizeResultViewModel
    {
        public RequestStatus RequestStatus { get; set; }

        public string Message { get; set; }

        public string UserFullName { get; set; }

        public string AccessTokens { get; set; }

        public string RefreshToken { get; set; }
    }

    public class AuthorizeTokenResultViewModel
    {
        public string AccessTokens { get; set; }

        public string RefreshToken { get; set; }
    }
}