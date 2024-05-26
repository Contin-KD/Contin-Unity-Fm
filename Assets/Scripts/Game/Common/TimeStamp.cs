namespace System
{
    /// <summary>
    /// 作者: Teddy
    /// 时间: 2018/09/04
    /// 功能: 
    /// </summary>
    public struct TimeStamp
    {
        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1);

        public static long Now
        {
            get
            {
                return ToTimeStamp(DateTime.UtcNow);
            }
        }

        public static long ToTimeStamp(DateTime dt)
        {
            TimeSpan ts = dt - EPOCH;
            return ts.Ticks / 10000L;
        }

        public static DateTime ToDateTime(long timestamp)
        {
            return EPOCH.AddTicks(timestamp * 10000L);
        }

        public static DateTime ToLocalDateTime(long timestamp)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(ToDateTime(timestamp), TimeZoneInfo.Local);
        }
    }
}