using TollCalculator.Models;
namespace TollCalculator.Interfaces
{
    public interface ITollManager
    {
        decimal CalculateVehicleToll(VehicleTypes vehicleType, DateTime[] dates);
        bool IsQueryValid(Query query);
    }
}
