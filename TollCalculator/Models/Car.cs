namespace TollCalculator.Models
{
    public class Car : Vehicle
    {
        public VehicleTypes GetVehicleType()
        {
            return VehicleTypes.Car;
        }
    }
}