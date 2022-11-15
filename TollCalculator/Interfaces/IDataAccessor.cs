using TollCalculator.Models;
namespace TollCalculator.Interfaces
    
{
    public interface IDataAccessor
    {
        decimal GetTollByTimeStampFromDB(DateTime date); 
        decimal GetDailyTollCellingFromDB();
        VehicleTypes[] GetTollFreeVehicleTypes();
    }
}
