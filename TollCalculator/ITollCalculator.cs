using Nager.Date;
using TollCalculator.Models;
namespace TollCalculator
{
    public interface ITollCalculator
    {
        /**
             * Calculate the total toll fee for one day
             *
             * @param vehicleType - the type of vehicle
             * @param dates   - date and time of all passes on one day
             * @return - the total congestion tax for that day
             */

        string CalculateToll(VehicleTypes vehicleType, DateTime[] dates);

    }
}