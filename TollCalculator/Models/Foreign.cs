namespace TollCalculator.Models
{
    public class Foreign : Vehicle
    {
        public VehicleTypes GetVehicleType()
        {
            return VehicleTypes.Foreign;
        }
    }
}