using System;

namespace PublicApiRepo.Utils
{
    public static class DateUtil
    {
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return new DateTime(
             dateTime.Year,
             dateTime.Month,
             dateTime.Day,
             23, 59, 59, 999);
        }
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return new DateTime(
             dateTime.Year,
             dateTime.Month,
             dateTime.Day,
             0, 0, 0, 0);
        }
    }
}
