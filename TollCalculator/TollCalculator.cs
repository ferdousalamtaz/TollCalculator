using Nager.Date;
using TollCalculator.Models;
namespace TollCalculator
{
    public class TollCalculator : ITollCalculator
    {
        private readonly DateTime _date;
        /**
             * Calculate the total toll fee for one day
             *
             * @param vehicleType - the type of vehicle
             * @param dates   - date and time of all passes on one day
             * @return - the total congestion tax for that day
             */

        public string CalculateToll(VehicleTypes vehicleType, DateTime[] dates)
        {
            var orderedDates = dates.OrderBy(x => x).ToList();

            DateTime intervalStart = dates[0];
            decimal totalFee = 0;
            decimal totalFeeCurrentDay = 0;
            decimal dailyFeeCeiling = 60;
            int uniqueDayCount = 1;

            foreach (DateTime date in orderedDates)
            {
                if (intervalStart.DayOfYear != date.DayOfYear)
                {
                    totalFee += totalFeeCurrentDay;
                    if (totalFee > (dailyFeeCeiling * uniqueDayCount))
                    {
                        totalFee = dailyFeeCeiling * uniqueDayCount;
                    }

                    totalFeeCurrentDay = 0;
                    intervalStart = date;
                    uniqueDayCount++;
                }

                decimal nextFee = GetTollFee(date, vehicleType);
                decimal tempFee = GetTollFee(intervalStart, vehicleType);

                TimeSpan diffInMillies = date - intervalStart;
                long minutes = (long)(diffInMillies.TotalMilliseconds / 1000 / 60);

                if (minutes <= 60)
                {
                    if (totalFeeCurrentDay > 0) totalFeeCurrentDay -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFeeCurrentDay += tempFee;
                }
                else
                {
                    totalFeeCurrentDay += nextFee;
                }
                intervalStart = date;
            }

            if (totalFeeCurrentDay > dailyFeeCeiling)
            {
                totalFeeCurrentDay = dailyFeeCeiling;
            }

            return (totalFee + totalFeeCurrentDay).ToString();
        }

        private readonly VehicleTypes[] tollFreeVehicles = { VehicleTypes.Emergency, VehicleTypes.Bus, VehicleTypes.Diplomat, VehicleTypes.Motorcycle, VehicleTypes.Motorcycle, VehicleTypes.Foreign };
        private decimal GetTollFee(DateTime date, VehicleTypes vehicleType)
        {
            if (IsTollFreeDate(date) || tollFreeVehicles.Contains(vehicleType)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 18 && minute > 29 || (hour > 18 && hour < 6)) return 0;
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

        private Boolean IsTollFreeDate(DateTime date)
        {
            return (date.DayOfWeek == DayOfWeek.Saturday ||
                date.DayOfWeek == DayOfWeek.Sunday ||
                date.Month == 7 ||
                DateSystem.IsPublicHoliday(date, CountryCode.SE) ||
                DateSystem.IsPublicHoliday(date.AddDays(1), CountryCode.SE));
        }
    }
}