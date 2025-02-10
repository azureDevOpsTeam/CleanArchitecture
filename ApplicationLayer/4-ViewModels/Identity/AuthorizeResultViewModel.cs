namespace ApplicationLayer.ViewModels.Identity
{
    public class AuthorizeResultViewModel
    {
        public string UserFullName { get; set; }

        public string SamAccountName { get; set; }

        public string AccessTokens { get; set; }

        public string RefreshToken { get; set; }
    }
}