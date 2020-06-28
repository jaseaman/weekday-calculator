using System;
using System.Linq.Expressions;

namespace WeekdayCalculator.Core.Model.HolidayDefinitions
{
    public partial class HolidayDefinition
    {
        public static Expression<Func<HolidayDefinition, bool>> BetweenDates(DateTime startDate, DateTime endDate) =>
            h => h.Month >= startDate.Month && h.Month <= endDate.Month;
    }
}