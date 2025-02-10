namespace ApplicationLayer.ViewModels.Identity
{
    public class LoginViewModel
    {
        public int ValidationMethod { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int SecurityCode { get; set; }
    }
}