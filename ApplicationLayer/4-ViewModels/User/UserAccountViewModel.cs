namespace ApplicationLayer.ViewModels
{
    public class UserAccountViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool ConfirmEmail { get; set; }

        public string PhoneNumber { get; set; }

        public bool ConfirmPhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string Company { get; set; }
    }
}