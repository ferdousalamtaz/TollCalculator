namespace TollCalculator.Models
{
    public class Military : Vehicle
    {
        public VehicleTypes GetVehicleType()
        {
            return VehicleTypes.Military;
        }
    }
}