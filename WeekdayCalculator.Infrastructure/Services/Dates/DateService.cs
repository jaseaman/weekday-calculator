using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeekdayCalculator.Core.Model.HolidayDefinitions;
using WeekdayCalculator.Core.Model.HolidayDefinitions.Util;
using WeekdayCalculator.Core.Services.Dates;
using WeekdayCalculator.Core.Util.Dates;
using WeekdayCalculator.Infrastructure.Repository;

namespace WeekdayCalculator.Infrastructure.Services.Dates
{
    public class DateService : IDateService
    {
        private readonly IReadRepository<HolidayDefinition, long> _holidayDefinitionRepository;

        public DateService(IReadRepository<HolidayDefinition, long> holidayDefinitionRepository)
        {
            _holidayDefinitionRepository = holidayDefinitionRepository;
        }

        /// <inheritdoc />
        public async Task<long> CalculateWorkingDaysExclusive(DateTime from, DateTime to) =>
            await CalculateWorkingDays(from.AddDays(1), to.AddDays(-1));

        /// <inheritdoc />
        public async Task<long> CalculateWorkingDays(DateTime from, DateTime to)
        {
            var totalDays = (long) (to - from).TotalDays + 1;
            if (totalDays <= 0) return 0;

            // Retrieve all relevant holidays definitions from data source, pull all if the total days elapses a year
            var holidays = await _holidayDefinitionRepository.Query
                .Where(totalDays < 365 ? HolidayDefinition.BetweenMonths(from, to) : h => true)
                .ToListAsync();

            // Efficiently calculate whole years, removing in batch. Skipping the last remainder of a year
            // Additional performance could be achieved with asynchronous batching
            var removalDays = 0L;
            var fullYears = totalDays / 365;
            for (var y = 0; y < fullYears; y++)
            {
                // Determine the count of all holidays that rest on a weekday
                removalDays = holidays
                    .Select(h => GetHolidayDate(from.Year + y, h).DayOfWeek)
                    .Aggregate(removalDays, (days, holidayDay) =>
                        days + (holidayDay != DayOfWeek.Saturday && holidayDay != DayOfWeek.Sunday ? 1 : 0));
            }

            // Calculate the overflow of days 
            var excessDays = totalDays % (DateTime.IsLeapYear(to.Year) ? 366 : 365);

            if (excessDays > 0)
                // Sum all holidays that exist within the partial range that are not on a weekend
                removalDays += holidays
                    .Where(h => h.Month >= to.AddDays(-excessDays).Month)
                    .Count(h =>
                    {
                        // Conditional as to what year the holiday falls on, i.e. 1/06/2020 - 1/05/2021
                        var holidayDate = GetHolidayDate(to.Year, h);
                        // Assume that if it isn't within the last year, it is in the previous year
                        if (holidayDate > to) holidayDate = GetHolidayDate(to.Year - 1, h);
                        return holidayDate >= from && holidayDate <= to &&
                               holidayDate.DayOfWeek != DayOfWeek.Saturday &&
                               holidayDate.DayOfWeek != DayOfWeek.Sunday;
                    });

            // Account for weekends in bulk
            var weekends = totalDays / 7 * 2;

            // Cycle through remaining days (Maximum 6) to determine how many additional weekends
            var remainingWeekOverflow = totalDays % 7;
            if (remainingWeekOverflow > 0)
            {
                var lastProcessedDate = to.AddDays(-remainingWeekOverflow);
                // TODO : There is a more optimal way, but in the interest of time the performance increase is negligible
                for (var d = 1; d <= remainingWeekOverflow; d++)
                {
                    var day = lastProcessedDate.AddDays(d).DayOfWeek;
                    if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday) weekends++;
                }
            }

            removalDays += weekends;
            return totalDays - removalDays;
        }

        private DateTime GetHolidayDate(int year, HolidayDefinition holidayDefinition)
        {
            return Enum.Parse<HolidayPlacementStrategy>(holidayDefinition.PlacementStrategy) switch
            {
                HolidayPlacementStrategy.FixedDate => DateUtils.CalculateFixedDatePlacement(year,
                    holidayDefinition.Month, holidayDefinition.Day),
                HolidayPlacementStrategy.FixedDay => DateUtils.CalculateFixedDayPlacement(year, holidayDefinition.Month,
                    holidayDefinition.DayOfWeek, holidayDefinition.WeekOfMonth),
                HolidayPlacementStrategy.FixedDateNonWeekend => DateUtils.CalculateFixedDateNonWeekendPlacement(year,
                    holidayDefinition.Month, holidayDefinition.Day),
                _ => throw new InvalidOperationException($"'{holidayDefinition.PlacementStrategy}' is not handled")
            };
        }
    }
}