using WeekdayCalculator.Core.Services.Dates;
using WeekdayCalculator.Infrastructure.Repository.EntityFramework;

namespace WeekdayCalculator.Infrastructure.Services.Dates
{
    public class DateService : IDateService
    {
        private readonly WeekdayCalculatorDbContext _context;
        
        public DateService(WeekdayCalculatorDbContext context)
        {
            _context = context;
        }
    }
}