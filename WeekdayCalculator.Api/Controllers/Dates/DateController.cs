using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeekdayCalculator.Api.Controllers.Dates.Requests;
using WeekdayCalculator.Api.Controllers.Dates.Responses;
using WeekdayCalculator.Core.Services.Dates;

namespace WeekdayCalculator.Api.Controllers.Dates
{
    [Route("api")]
    public class DateController : ControllerBase
    {
        private readonly IDateService _dateService;
        
        public DateController(IDateService dateService)
        {
            _dateService = dateService;
        }
        
        [HttpPost("calculateDays")]
        public async Task<DaysCalculationResponse> CalculateDaysInRange([FromBody] DaysCalculationRequest request)
        {
            return new DaysCalculationResponse();
        }
    }
}