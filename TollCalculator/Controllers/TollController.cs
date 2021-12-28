using Microsoft.AspNetCore.Mvc;
using TollCalculator.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TollCalculator
{
    [ApiController]
    [Route("TollCalculator")]    
    public class TollController : ControllerBase
    {
        private readonly IExtension extension;
        public TollController(IExtension extension)
        { 
            this.extension = extension;
        }

        [HttpPost("{query}")]
        public ActionResult<decimal> CalculateToll(Query query)
        {
            if (!extension.IsQueryValid(query))
            {
                return BadRequest();
            }
            return extension.CalculateToll(query.vehicleType, query.passingDates);
        }
    }
}
