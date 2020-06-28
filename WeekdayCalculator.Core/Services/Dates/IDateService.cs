using System;
using System.Threading.Tasks;

namespace WeekdayCalculator.Core.Services.Dates
{
    public interface IDateService
    {
        Task<long> CalculateWorkingDaysExclusive(DateTime from, DateTime to);
    }
}