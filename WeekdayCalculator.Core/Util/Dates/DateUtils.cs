using System;
using WeekdayCalculator.Core.Model.HolidayDefinitions.Util;

namespace WeekdayCalculator.Core.Util.Dates
{
    public class DateUtils
    {
        public static DateTime CalculateFixedDayPlacement(int year, int month, DayOfWeek? dayOfWeek, int? week)
        {
            if (dayOfWeek == null)
                throw new InvalidOperationException(
                    $"Day of week must be defined for '{nameof(HolidayPlacementStrategy.FixedDay)}' placement strategy");
            if (week == null)
                throw new InvalidOperationException(
                    $"Week of month must be defined for '{nameof(HolidayPlacementStrategy.FixedDay)}' placement strategy");

            return new DateTime(year, month, 1).GetNthWeekDay(dayOfWeek.Value, week.Value);
        }

        public static DateTime CalculateFixedDatePlacement(int year, int month, int? day)
        {
            if (day == null)
                throw new InvalidOperationException(
                    $"Day must be defined for '{nameof(HolidayPlacementStrategy.FixedDate)}' placement strategy");
            return new DateTime(year, month, day.Value);
        }

        public static DateTime CalculateFixedDateNonWeekendPlacement(int year, int month, int? day)
        {
            if (day == null)
                throw new InvalidOperationException(
                    $"Day must be defined for '{nameof(HolidayPlacementStrategy.FixedDateNonWeekend)}' placement strategy");
            var initialDate = new DateTime(year, month, day.Value);
            return initialDate
                .AddDays(initialDate.DayOfWeek == DayOfWeek.Sunday ? 1 : 0)
                .AddDays(initialDate.DayOfWeek == DayOfWeek.Saturday ? 2 : 0);
        }
    }
}