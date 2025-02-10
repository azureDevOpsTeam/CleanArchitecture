namespace ApplicationLayer.ViewModels.Identity
{
    public class ChangePasswordWithRecovery
    {
        public string EmailOrUserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}