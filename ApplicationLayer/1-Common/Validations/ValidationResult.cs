namespace ApplicationLayer.Common.Validations
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public object ValidatedValue { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}