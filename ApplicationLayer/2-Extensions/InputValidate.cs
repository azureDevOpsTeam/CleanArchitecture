#region Usings

using System.Text.RegularExpressions;

#endregion

namespace ApplicationLayer.Extensions
{
    public static class InputValidate
    {
        public static bool IsValidEmail(string input)
        {
            // Regular expression pattern for email validation
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsValidMobile(string input)
        {
            // Regular expression pattern for mobile number validation
            string pattern = @"^(09|9\d{9})$"; // Assumes a 10-digit mobile number
            return Regex.IsMatch(input, pattern);
        }
    }
}