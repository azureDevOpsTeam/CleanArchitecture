using Ardalis.SmartEnum;

namespace ApplicationLayer.Extensions.SmartEnums
{
    public class DayOfWeekEnum : SmartEnum<DayOfWeekEnum>
    {
        public static readonly DayOfWeekEnum Sunday = new(0, "یک‌شنبه", "Sunday");
        public static readonly DayOfWeekEnum Monday = new(1, "دوشنبه", "Monday");
        public static readonly DayOfWeekEnum Tuesday = new(2, "سه‌شنبه", "Tuesday");
        public static readonly DayOfWeekEnum Wednesday = new(3, "چهارشنبه", "Wednesday");
        public static readonly DayOfWeekEnum Thursday = new(4, "پنج‌شنبه", "Thursday");
        public static readonly DayOfWeekEnum Friday = new(5, "جمعه", "Friday");
        public static readonly DayOfWeekEnum Saturday = new(6, "شنبه", "Saturday");

        public string PersianName { get; }

        public string EnglishName { get; }

        private DayOfWeekEnum(int id, string persianName, string englishName) : base(englishName, id)
        {
            PersianName = persianName;
            EnglishName = englishName;
        }
    }
}