using TollCalculator.Models;

namespace TollCalculator
{
    public class DatabaseProvider
    {
        private List<TaxationInterval> taxationIntervals = new List<TaxationInterval>
            {
                new TaxationInterval
                {
                    Start = TimeOnly.Parse("06:00 AM"),
                    End = TimeOnly.Parse("06:30 AM"),
                    Tax = 18
                }
            };

        private List<TaxationInterval> TaxationIntervals { get => taxationIntervals; set => taxationIntervals = value; }

        public List<TaxationInterval> GetTaxationIntervals()
        {
            return TaxationIntervals;
        }
    }
}
