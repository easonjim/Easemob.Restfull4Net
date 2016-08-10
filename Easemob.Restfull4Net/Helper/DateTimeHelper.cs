using System;

namespace Easemob.Restfull4Net.Helper
{
    public static class DateTimeHelper
    {
        private static DateTime _baseTime = new DateTime(1970, 1, 1);//Unix起始时间

        /// <summary>
        /// 转换时间戳到C#时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        public static DateTime GetDateTime(long timeStamp)
        {
            return _baseTime.AddTicks((timeStamp + 8 * 60 * 60) * 10000000);
        }

        /// <summary>
        /// 转换时间戳到C#时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        public static DateTime GetDateTime(string timeStamp)
        {
            return GetDateTime(long.Parse(timeStamp));
        }

        /// <summary>
        /// 获取时间戳（UNIX时间戳）
        /// </summary>
        /// <param name="dateTime">时间</param>
        public static long GetTimeStamp(DateTime dateTime)
        {
            return (dateTime.Ticks - _baseTime.Ticks) / 10000000 - 8 * 60 * 60;
        }
    }
}
