using TollCalculator.Interfaces;
using TollCalculator.Models;
namespace TollCalculator.Providers
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly decimal _dailyTollCeiling = 60;

        public decimal GetDailyTollCellingFromDB()
        {
            return _dailyTollCeiling;
        }

        public decimal GetTollByTimeStampFromDB(DateTime date)
        {
            // In practice we will Fetch a list of TimeIntervals from DB and match the incoming date to get the tax.
            int hour = date.Hour;
            int minute = date.Minute;

            if(hour == 18 && minute > 29 || (hour > 18 && hour < 6)) return 0;
            else if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 30 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        
        //Fake db data
        private readonly VehicleTypes[] _tollFreeVehicleTypes = new VehicleTypes[]
        {   VehicleTypes.Motorcycle,
            VehicleTypes.Emergency,
            VehicleTypes.Diplomat,
            VehicleTypes.Foreign,
            VehicleTypes.Military,
            VehicleTypes.Bus
        };
        public bool IsVehicleTollFreeFromDB(VehicleTypes vehicleType)
        {
            return _tollFreeVehicleTypes.Contains(vehicleType);
        }
    }
}
