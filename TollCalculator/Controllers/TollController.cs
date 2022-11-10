using Microsoft.AspNetCore.Mvc;

namespace TollCalculator.Models
{
    [ApiController]
    [Route("TollCalculator")]    
    public class TollController : ControllerBase
    {
        private readonly ITollCalculator _tollCalculator;
        public TollController(ITollCalculator tollCalculator)
        {
            _tollCalculator = tollCalculator;
        }
        [HttpGet]
        public ActionResult<string> CalculateToll( [FromQuery] Query query)
        {
            if (query == null)
            {
                return BadRequest();
            }
            return _tollCalculator.CalculateToll(query.vehicleType, query.passingDates);
        }
    }
}
