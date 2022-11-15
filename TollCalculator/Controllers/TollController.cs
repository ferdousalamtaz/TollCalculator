using Microsoft.AspNetCore.Mvc;
using TollCalculator.Models;
using TollCalculator.Interfaces;
using TollCalculator.Business;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TollCalculator
{
    [ApiController]
    [Route("TollCalculator")]
    public class TollController : ControllerBase
    {
        private readonly ILogger<TollController> _logger;
        private readonly ITollManager _tollManager;

        public TollController(ITollManager manager, ILogger<TollController> logger)
        {
            _logger = logger;
            _tollManager = manager;
        }


        /**
         * <summary>Calculate the toll for multiple days</summary>         
         * <param name="query">The query parameter contains a list of DateTime and a VehcleType in string</param>        
         * <returns>decimal - the total congestion tax for provided days </returns>
         */
        [HttpGet]
        public ActionResult<decimal> CalculateToll([FromQuery] Query query)
        {
            if (!_tollManager.IsQueryValid(query))
            {               
               return BadRequest("Please provide a valid vehicleType and past dates.");
            }

            try
            {
                decimal toll = _tollManager.CalculateVehicleToll(query.vehicleType, query.passingDates);
                return Ok(toll);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate toll due to exception.");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
