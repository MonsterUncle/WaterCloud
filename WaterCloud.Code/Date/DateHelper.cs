using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    /// <summary>
    /// 
    /// </summary>
    public class DateHelper
    {
        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        //// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        //// <summary>
        /// 取得上个月第一天
        /// </summary>
        /// <param name="datetime">要取得上个月第一天的当前时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfPreviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }

        //// <summary>
        /// 取得上个月的最后一天
        /// </summary>
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfPreviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }
        /// <summary>
        /// 取得某周的第一天
        /// </summary>
        /// <param name="datetime">要取得周第一天的时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(DateTime datetime)
        {
            return datetime.AddDays(0 - Convert.ToInt16(datetime.DayOfWeek) + 1);
        }
        /// <summary>
        /// 取得某周的第一天
        /// </summary>
        /// <param name="datetime">要取得周第一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfWeek(DateTime datetime)
        {
            return datetime.AddDays(6 - Convert.ToInt16(datetime.DayOfWeek) + 1);
        }
    }
}
