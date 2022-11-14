using System.ComponentModel.DataAnnotations;

namespace TollCalculator.Models
{
    public class Query
    {
        [Required]
        public VehicleTypes vehicleType { get; set; }
        [Required]        
        public DateTime[] passingDates { get; set; }
    }
}