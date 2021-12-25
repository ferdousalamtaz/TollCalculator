namespace TollCalculator.Models
{
    public class Motorcycle : Vehicle
    {
        public VehicleTypes GetVehicleType()
        {
            return VehicleTypes.Motorcycle;
        }
    }
}