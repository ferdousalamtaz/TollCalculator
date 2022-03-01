using Nager.Date;
using TollCalculator.Models;
using TollCalculator.Interfaces;
namespace TollCalculator.Business
{

    public class TollManager : ITollManager
    {
        private readonly IDatabaseProvider _databaseProvider;

        public TollManager(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public decimal CalculateVehicleToll(VehicleTypes vehicleType, DateTime[] dates)
        {
            List<DateTime>? orderedDates = dates.OrderBy(x => x).ToList();

            DateTime intervalStart = dates[0];
            decimal totalToll = 0;
            decimal totalTollCurrentDay = 0;
            int uniqueDayCount = 1;

            if (_databaseProvider.IsVehicleTollFreeFromDB(vehicleType)) return 0;

            decimal dailyTollCeiling = _databaseProvider.GetDailyTollCellingFromDB();


            foreach (DateTime date in orderedDates)
            {
                if (DateTime.Compare(intervalStart.Date, date.Date) != 0)
                {
                    totalToll += totalTollCurrentDay;
                    if (totalToll > (dailyTollCeiling * uniqueDayCount))
                    {
                        totalToll = dailyTollCeiling * uniqueDayCount;
                    }

                    totalTollCurrentDay = 0;
                    intervalStart = date;
                    uniqueDayCount++;
                }

                decimal nextToll = IsTollFreeDate(date) ? 0 : _databaseProvider.GetTollByTimeStampFromDB(date);
                decimal tempToll = IsTollFreeDate(intervalStart) ? 0 : _databaseProvider.GetTollByTimeStampFromDB(intervalStart);

                TimeSpan diffInMillies = date - intervalStart;
                long minutes = (long)(diffInMillies.TotalMilliseconds / 1000 / 60);

                if (minutes <= 60)
                {
                    if (totalTollCurrentDay > 0) totalTollCurrentDay -= tempToll;
                    if (nextToll >= tempToll) tempToll = nextToll;
                    totalTollCurrentDay += tempToll;
                }
                else
                {
                    totalTollCurrentDay += nextToll;
                }
                intervalStart = date;
            }

            if (totalTollCurrentDay > dailyTollCeiling)
            {
                totalTollCurrentDay = dailyTollCeiling;
            }

            return (totalToll + totalTollCurrentDay);
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

            List<DateTime>? days = query?.passingDates?.ToList();
            Boolean invalidDate = false;


            days?.ForEach(d =>
            {
                if (DateTime.Compare(d, DateTime.Today) > 0)
                {
                    invalidDate = true;
                }
            });
            if (invalidDate) return false;
            else return true;
        }
    }
}