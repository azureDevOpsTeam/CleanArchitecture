#region Usings

using Ardalis.SmartEnum;

#endregion

namespace ApplicationLayer.Extensions.SmartEnums
{
    public sealed class ValidationMethod(string name, int value) : SmartEnum<ValidationMethod>(name, value)
    {
        #region Field

        public static ValidationMethod UserInformation = new("اطلاعات کاربری", 1);

        public static ValidationMethod OneTimePasswordMobile = new("رمز یکبار مصرف موبایل", 2);

        public static ValidationMethod OneTimePasswordEmail = new("رمز یکبار مصرف ایمیل", 3);

        #endregion Field
    }
}