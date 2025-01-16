using System;

namespace QFramework.Extenstions
{
    public static class DateTimeExtensions
    {
        private static DateTime s_originalTime = new DateTime(1970, 1, 1, 0, 0, 0);

        public static string ToShortString(this DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmss");
        }

        public static string ToLongString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static long GetTimestamp(this DateTime dt)
        {
            return (long) ( dt.ToUniversalTime() - s_originalTime ).TotalSeconds;
        }
        public static long GetTimestampMillsec(this DateTime dt)
        {
            return (long)(dt.ToUniversalTime() - s_originalTime).TotalMilliseconds;
        }
    }
}