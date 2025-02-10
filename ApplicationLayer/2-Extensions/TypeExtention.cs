#region Usings

using System.Globalization;

#endregion

namespace ApplicationLayer.Extensions
{
    public static class TypeExtention
    {
        #region Methods

        #region To Byte Int & Double & Long & Float

        public static byte ToByte(this object value)
        {
            if (value == null)
                return 0;
            byte.TryParse(value.ToString(), out var result);
            return result;
        }

        public static int ToInt(this object value)
        {
            if (value == null)
                return 0;
            int.TryParse(value.ToString(), out var result);
            return result;
        }

        public static decimal ToDecimal(this object value)
        {
            if (value == null)
                return 0;
            decimal.TryParse(value.ToString(), out var result);
            return result;
        }

        public static double ToDouble(this object value)
        {
            if (value == null)
                return 0;
            double.TryParse(value.ToString(), out var result);
            return result;
        }

        public static long ToLong(this object value)
        {
            if (value == null)
                return 0;
            long.TryParse(Math.Round(value.ToDouble(), 0).ToString(CultureInfo.InvariantCulture), out var result);
            return result;
        }

        public static short ToShort(this object value)
        {
            if (value == null)
                return 0;
            short.TryParse(Math.Round(value.ToDouble(), 0).ToString(CultureInfo.InvariantCulture), out var result);
            return result;
        }

        public static float ToFloat(this object value)
        {
            if (value == null)
                return 0;
            float.TryParse(value.ToString(), out var result);
            return result;
        }

        #endregion To Byte Int & Double & Long & Float

        #region To DateTime

        public static DateTime ToDateTime(this object value)
        {
            if (value == null)
                return DateTime.MinValue;
            DateTime.TryParse(value.ToString(), out var result);
            return result;
        }

        public static DateTime? ToDateTimeNullable(this object value)
        {
            if (value == null)
                return null;
            DateTime.TryParse(value.ToString(), out var result);
            return result;
        }

        #endregion To DateTime

        #region To Boolean

        public static bool ToBoolean(this object value)
        {
            if (value == null)
                return false;
            bool.TryParse(value.ToString(), out var result);
            return result;
        }

        #endregion To Boolean

        #region ToCurrency

        public static string ToCurrency(this decimal value) => value.ToString("N0", CultureInfo.InvariantCulture);

        public static string ToCurrency(this decimal? value) => value?.ToString("N0", CultureInfo.InvariantCulture);

        #endregion ToCurrency

        #region To LowerWithTrim

        public static string ToLowerWithTrim(this string value) => value.Trim().ToLower();

        #endregion To LowerWithTrim

        #endregion Methods
    }
}