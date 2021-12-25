using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TollCalculator.Models
{
    [ApiController]
    [Route("TollCalculator")]    
    public class TollController : ControllerBase
    {
        [HttpPost("{query}")]
        public ActionResult<string> CalculateToll(Query query)
        {
            if (query == null )
            {
                return BadRequest();
            }
            return Extensions.CalculateToll(query.vehicleType, query.passingDates);
        }
    }
}
