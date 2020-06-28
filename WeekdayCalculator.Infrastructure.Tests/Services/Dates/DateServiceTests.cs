using System;
using System.Threading.Tasks;
using NUnit.Framework;
using WeekdayCalculator.Core.Services.Dates;

namespace WeekdayCalculator.Core.Tests.Services.Dates
{
    [TestFixture]
    public class DateServiceTests
    {
        private IDateService _dateService;

        private static readonly object[] TestWeekdaysCalculationTestCases =
        {
            // - Thu 7/8/2014 to Mon 11/8/2014 should return 1
            new object[] {new DateTime(2014, 8, 7), new DateTime(2014, 08, 11), 1},
            // - Wed 13/8/2014 to Thu 21/8/2014 should return 5
            new object[] {new DateTime(2014, 8, 13), new DateTime(2014, 8, 21), 5}
        };
        
        [TestCaseSource(nameof(TestWeekdaysCalculationTestCases))]
        public async Task TestWeekdaysCalculation(DateTime from, DateTime to, long expectedResult)
        {
            
        }
    }
}