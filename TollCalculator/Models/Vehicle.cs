namespace TollCalculator.Models
{
    public interface Vehicle
    {
        VehicleTypes GetVehicleType();
    }
   
    public enum VehicleTypes
    {
        Motorcycle = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5,
        Bus = 6,
        Car = 7,
    }

    public enum TollFreeVehicles
    {
        Motorcycle = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5,
        Bus = 6,
    }
}