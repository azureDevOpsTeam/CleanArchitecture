namespace ApplicationLayer.ViewModels.Identity
{
    public class SignUpViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string PostalCode { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public bool? Gender { get; set; }

        public int? MaritalStatus { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool ConfirmEmail { get; set; }

        public bool ConfirmPhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; }
    }
}