namespace TollCalculator.Models
{
    public class Bus : Vehicle
    {
        public VehicleTypes GetVehicleType()
        {
            return VehicleTypes.Bus;
        }
    }
}