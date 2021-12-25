namespace TollCalculator.Models
{
    public class Query
    {
        public VehicleTypes vehicleType { get; set; }

        public DateTime[] passingDates { get; set; }
    }
}