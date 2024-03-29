﻿using TollCalculator.Interfaces;
using TollCalculator.Models;
namespace TollCalculator.Providers
{
    public class DataAccssor : IDataAccessor
    {
        private readonly decimal _dailyTollCeiling = 60;

        public decimal GetDailyTollCellingFromDB()
        {
            return _dailyTollCeiling;
        }        
        
        private readonly VehicleTypes[] _tollFreeVehicleTypes = new VehicleTypes[]
        {   VehicleTypes.Motorcycle,
            VehicleTypes.Emergency,
            VehicleTypes.Diplomat,
            VehicleTypes.Foreign,
            VehicleTypes.Military,
            VehicleTypes.Bus
        };
        public VehicleTypes[] GetTollFreeVehicleTypes()
        {
            return _tollFreeVehicleTypes;
        }

        public decimal GetTollByTimeStampFromDB(DateTime date)
        {
            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 18 && minute > 29 || (hour > 18 && hour < 6)) return 0;
            else if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if ((hour >= 8 && minute >= 30 ) && (hour <= 14 && minute <= 59)) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 30 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }
    }
}
