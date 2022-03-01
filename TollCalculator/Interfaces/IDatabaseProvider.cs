using TollCalculator.Models;
namespace TollCalculator.Interfaces
    
{
    public interface IDatabaseProvider
    {
        decimal GetTollByTimeStampFromDB(DateTime date);
        decimal GetDailyTollCellingFromDB();
        bool IsVehicleTollFreeFromDB(VehicleTypes vehicleType);
    }
}
