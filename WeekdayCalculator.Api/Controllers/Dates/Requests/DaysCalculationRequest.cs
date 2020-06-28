using System;

namespace WeekdayCalculator.Api.Controllers.Dates.Requests
{
    public class DaysCalculationRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}