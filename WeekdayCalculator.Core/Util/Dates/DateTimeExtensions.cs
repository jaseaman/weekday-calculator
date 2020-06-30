using System;

namespace WeekdayCalculator.Core.Util.Dates
{
    public static class DateTimeExtensions
    {
        public static DateTime GetNthWeekDay(this DateTime d, DayOfWeek day, int week) => 
            d.AddDays((day < d.DayOfWeek ? 7 : 0) + day - d.DayOfWeek + week * 7);
    }
}