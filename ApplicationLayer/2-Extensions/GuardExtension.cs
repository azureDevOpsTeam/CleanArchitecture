#region Usings

using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

#endregion

namespace ApplicationLayer.Extensions
{
    public static class GuardExtension
    {
        #region Methods

        public static void CheckArgumentIsNull<T>(this T o)
        {
            if (o == null) throw new ArgumentNullException(typeof(T).Name);
        }

        public static bool IsEmailAddress(this string inputText) => !string.IsNullOrWhiteSpace(inputText) && new EmailAddressAttribute().IsValid(inputText);

        public static bool ContainsNumber(this string inputText) => !string.IsNullOrWhiteSpace(inputText) && inputText.ToEnglishNumbers().Any(char.IsDigit);

        public static bool IsNumeric(this string inputText) => string.IsNullOrWhiteSpace(inputText) && long.TryParse(inputText.ToEnglishNumbers(), out _);

        public static bool HasConsecutiveChars(this string inputText, int sequenceLength = 3)
        {
            var charEnumerator = StringInfo.GetTextElementEnumerator(inputText);
            var currentElement = string.Empty;
            var count = 1;
            while (charEnumerator.MoveNext())
            {
                if (currentElement == charEnumerator.GetTextElement())
                {
                    if (++count >= sequenceLength)
                        return true;
                }
                else
                {
                    count = 1;
                    currentElement = charEnumerator.GetTextElement();
                }
            }
            return false;
        }

        #endregion Methods
    }
}