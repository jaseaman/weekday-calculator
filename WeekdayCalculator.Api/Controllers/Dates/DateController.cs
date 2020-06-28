using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeekdayCalculator.Api.Controllers.Dates.Requests;
using WeekdayCalculator.Api.Controllers.Dates.Responses;

namespace WeekdayCalculator.Api.Controllers.Dates
{
    [Route("api")]
    public class DateController : ControllerBase
    {
        [HttpPost("calculateDays")]
        public Task<DaysCalculationResponse> CalculateDaysInRange([FromBody] DaysCalculationRequest request)
        {
            return null;
        }
    }
}