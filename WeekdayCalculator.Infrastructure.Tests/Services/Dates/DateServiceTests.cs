using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Shouldly;
using WeekdayCalculator.Core.Model.HolidayDefinitions;
using WeekdayCalculator.Core.Services.Dates;
using WeekdayCalculator.Infrastructure.Repository.EntityFramework;
using WeekdayCalculator.Infrastructure.Services.Dates;

namespace WeekdayCalculator.Core.Tests.Services.Dates
{
    [TestFixture]
    public class DateServiceTests
    {
        private IDateService _dateService;

        private static readonly object[] TestWeekdaysCalculationTestCases =
        {
            // Test Ending Weekend- Thu 7/8/2014 to Mon 11/8/2014 should return 1, no holidays
            new object[] {new DateTime(2014, 8, 7), new DateTime(2014, 08, 11), 1},
            // Test Weekend Middle - Wed 13/8/2014 to Thu 21/8/2014 should return 5, no holidays
            new object[] {new DateTime(2014, 8, 13), new DateTime(2014, 8, 21), 5},
            // Test Only Weekend - Fri 26/06/2020 to Mon 29/06/2020 should return 0, no holidays
            new object[] {new DateTime(2020, 6, 26), new DateTime(2020, 6, 29), 0,},
            // Test 2.2 Weeks - Wed 6/05/2020 to Sat 23/05/2020 should return 12, no holidays
            new object[] {new DateTime(2020, 5, 6), new DateTime(2020, 5, 23), 12},
            // Test Starting Sat / Ending Mon, more than 1 week - Sat 20/06/2020 to Mon 29/06/2020
            new object[] {new DateTime(2020, 6, 20), new DateTime(2020, 6, 29), 5,},
            // Test Starting Fri / Ending Sun - Fri 19/06/2020 to Sun 28/06/2020
            new object[] {new DateTime(2020, 6, 19), new DateTime(2020, 6, 28), 5},
            // Test with a weekend in the middle of the range
            new object[] {new DateTime(2020, 6, 11), new DateTime(2020, 6, 16), 2},
            // Test for full month with holiday - Sun 31/05/2020 - Wed 1/07/2020, should return 22
            new object[] {new DateTime(2020, 5, 31), new DateTime(2020, 7, 1), 21},
            // Test for full year - Tue 31/12/2020 - Fri 1/1/2020, should return 
            new object[] {new DateTime(2019, 12, 31), new DateTime(2021, 1, 1), 258},
            // Test same day, should return 0
            new object[] {new DateTime(2020, 6, 25), new DateTime(2020, 6, 25), 0},
            // Test with single fixed date holiday Christmas, should return 22
            new object[] {new DateTime(2020, 11, 30), new DateTime(2021, 1, 1), 22},
            // Test short range of years
            new object[] {new DateTime(2017, 5, 30), new DateTime(2020, 5, 17), 755},
            // Test long range of years
            new object[] {new DateTime(1900, 5, 30), new DateTime(2020, 5, 17), 30588}
        };

        [OneTimeSetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("Config/Secrets/Secrets.test.json", false)
                .Build();

            var connectionString =
                $@"User ID={configuration["Database:Username"]}; Password={configuration["Database:Password"]}; 
                Host={configuration["Database:Endpoint"]}; Port={configuration["Database:Port"]};
                Database={configuration["Database:Name"]}; Pooling=true;";
            var contextOptions = new DbContextOptionsBuilder<WeekdayCalculatorDbContext>()
                .UseNpgsql(connectionString)
                .Options;
            var context = new WeekdayCalculatorDbContext(contextOptions);
            _dateService = new DateService(new EntityFrameworkRepository<HolidayDefinition, long>(context));
        }

        [TestCaseSource(nameof(TestWeekdaysCalculationTestCases))]
        public async Task TestWeekdaysCalculation(DateTime from, DateTime to, long expectedResult)
        {
            (await _dateService.CalculateWorkingDaysExclusive(from, to)).ShouldBe(expectedResult);
        }
    }
}