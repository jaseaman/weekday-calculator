using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeekdayCalculator.Core.Model.HolidayDefinitions;
using WeekdayCalculator.Core.Services.Dates;
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

        public async Task<long> CalculateWorkingDaysExclusive(DateTime from, DateTime to) =>
            await CalculateWorkingDays(from.AddDays(1), to.AddDays(-1));


        public async Task<long> CalculateWorkingDays(DateTime from, DateTime to)
        {
            // Calculate total days exclusive
            var totalDays = (long) (to - from).TotalDays + 1;

            // Retrieve all relevant holidays definitions
            var holidays = await _holidayDefinitionRepository.Query.Where(totalDays >= 365
                ? HolidayDefinition.BetweenDates(from, to)
                : h => true).ToListAsync();

            // Efficiently calculate whole years, removing in batch. Skipping the first and last years (as they may be partial)
            var removalDays = 0L;
            for (var y = from.Year + 1; y <= to.Year - 1; y++)
            {
                // TODO : Remove holidays that are on weekends
                removalDays += holidays.Count;
            }
            
            // Remove holidays for the start portion
            foreach (var h in holidays)
            {
                
            }
            
            // Remove holidays for the end portion

            // Account for weekends
            var weekends = totalDays / 7 * 2
                + (from.DayOfWeek == DayOfWeek.Saturday || to.DayOfWeek == DayOfWeek.Sunday ? 2 : 0)
                + (from.DayOfWeek == DayOfWeek.Sunday || to.DayOfWeek == DayOfWeek.Saturday ? 1 : 0);
            removalDays += weekends;
            // Deal with partial years (Beginning slice and final slice)
            return totalDays - removalDays;
        }
    }
}