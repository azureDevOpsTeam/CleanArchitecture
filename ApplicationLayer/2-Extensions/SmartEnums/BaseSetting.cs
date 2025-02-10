using Ardalis.SmartEnum;

namespace ApplicationLayer.Extensions.SmartEnums
{
    public class BaseSetting : SmartEnum<BaseSetting>
    {
        public static readonly BaseSetting BaseExpressPrice = new(0, "مبلغ اکسپرس", "BaseExpressPrice");

        public string PersianName { get; }

        public string EnglishName { get; }

        private BaseSetting(int id, string persianName, string englishName) : base(englishName, id)
        {
            PersianName = persianName;
            EnglishName = englishName;
        }
    }
}