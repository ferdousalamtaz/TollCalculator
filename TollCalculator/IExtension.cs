using TollCalculator.Models;
namespace TollCalculator
{
    public interface IExtension
    {
        decimal CalculateToll(VehicleTypes vehicleType, DateTime[] dates);
        bool IsQueryValid(Query query);
    }
}
