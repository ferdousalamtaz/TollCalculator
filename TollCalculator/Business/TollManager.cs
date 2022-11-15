using Nager.Date;
using TollCalculator.Models;
using TollCalculator.Interfaces;
namespace TollCalculator.Business;

public class TollManager : ITollManager
{
    private readonly IDataAccessor _dataAccessor;
    private VehicleTypes[]? tollFreeVehicleTypes{get;set;}
    private decimal dailyTollCeiling { get; set; }

    public TollManager(IDataAccessor databaseAccessor)
    {
        _dataAccessor = databaseAccessor;
    }

    public decimal CalculateVehicleToll(VehicleTypes vehicleType, DateTime[] dates)
    {
        List<DateTime>? orderedDates = dates.OrderBy(x => x).ToList();

        DateTime intervalStart = dates[0];
        decimal totalToll = 0;
        decimal totalTollCurrentDay = 0;
        int uniqueDayCount = 1;
        tollFreeVehicleTypes = _dataAccessor.GetTollFreeVehicleTypes();
        
        if (tollFreeVehicleTypes.Contains(vehicleType)) return 0;

        dailyTollCeiling = _dataAccessor.GetDailyTollCellingFromDB();

        foreach (DateTime date in orderedDates)
        {
            if (DateTime.Compare(intervalStart.Date, date.Date) != 0)
            {
                totalToll += totalTollCurrentDay;
                if (totalToll > (dailyTollCeiling * uniqueDayCount))
                {
                    totalToll = dailyTollCeiling * uniqueDayCount;
                    intervalStart = date;
                    continue;
                }

                totalTollCurrentDay = 0;
                intervalStart = date;
                uniqueDayCount++;
            }

            decimal nextToll = IsTollFreeDate(date) ? 0 : _dataAccessor.GetTollByTimeStampFromDB(date);
            decimal tempToll = IsTollFreeDate(intervalStart) ? 0 : _dataAccessor.GetTollByTimeStampFromDB(intervalStart);

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

    private Boolean IsTollFreeDate(DateTime date)
    {
        return (date.Month == 7 ||
            date.DayOfWeek == DayOfWeek.Sunday ||
            date.DayOfWeek == DayOfWeek.Saturday ||
            DateSystem.IsPublicHoliday(date, CountryCode.SE) ||
            DateSystem.IsPublicHoliday(date.AddDays(1), CountryCode.SE));
    }

    public bool IsQueryValid(Query query)
    {
        if (query == null) return false;
        
       foreach (var d in query.passingDates.ToList())
        {
            if (DateTime.Compare(d, DateTime.Today) > 0)
            {
                return false;
            }
        }
       return true;
    }
}
