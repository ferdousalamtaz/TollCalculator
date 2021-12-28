using Nager.Date;
using TollCalculator.Models;
namespace TollCalculator
{
            /**
             * Calculate the total toll fee for multiple days
             *
             * @param vehicleType - the type of vehicle
             * @param dates   - date and time of all passes on one day
             * @return - the total congestion tax for that day in decimal
             */
    public class Extensions:IExtension
    {
        private readonly VehicleTypes[] TollFreeVehicleTypes = new VehicleTypes[] 
        {   VehicleTypes.Motorcycle,
            VehicleTypes.Emergency,
            VehicleTypes.Diplomat, 
            VehicleTypes.Foreign,
            VehicleTypes.Military,
            VehicleTypes.Bus 
        };       

        public decimal CalculateToll(VehicleTypes vehicleType, DateTime[] dates)
        {
            var orderedDates = dates.OrderBy(x => x).ToList();

            DateTime intervalStart = dates[0];
            decimal totalFee = 0;
            decimal totalFeeCurrentDay = 0;
            decimal dailyFeeCeiling = 60;
            int uniqueDayCount = 1;

            foreach (DateTime date in orderedDates)
            {
                if (DateTime.Compare(intervalStart, date) != 0)
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

            return (totalFee + totalFeeCurrentDay);
        }

        public decimal GetTollFee(DateTime date, VehicleTypes vehicleType)
        {
            if (IsTollFreeDate(date) || TollFreeVehicleTypes.Contains(vehicleType)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
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

        private bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (DateSystem.IsPublicHoliday(date, CountryCode.SE)
                || DateSystem.IsPublicHoliday(date.AddDays(1), CountryCode.SE)
                || date.Month == 7
                )
            {
                return true;
            }
            return false;
        }

        public bool IsQueryValid(Query query)
        {
            if (query == null
                || (query.passingDates == null || query.passingDates?.Length == 0)
                || !Enum.IsDefined(typeof(VehicleTypes), query.vehicleType))
            {
                return false;
            }
            return true;
        }
    }
}