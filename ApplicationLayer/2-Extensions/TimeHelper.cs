namespace ApplicationLayer.Extensions
{
    public static class TimeHelper
    {
        public static DateTime UnixTimeStampToDateTime(long UnixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(UnixTimeStamp);
            return dateTimeVal;
        }
    }
}
