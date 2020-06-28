using Microsoft.EntityFrameworkCore;
using WeekdayCalculator.Core.Model.HolidayDefinitions;

namespace WeekdayCalculator.Infrastructure.Repository.EntityFramework
{
    public class WeekdayCalculatorDbContext : DbContext
    {
        public DbSet<HolidayDefinition> HolidayDefinitions { get; set; }

        public WeekdayCalculatorDbContext(DbContextOptions<WeekdayCalculatorDbContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
            base.OnConfiguring(optionsBuilder);
        }
    }
}