namespace TollCalculator.Models
{
    public class Emergency : Vehicle
    {
        public VehicleTypes GetVehicleType()
        {
            return VehicleTypes.Emergency;
        }
    }
}