namespace TollCalculator.Models
{
    public class TaxationInterval
    {
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public decimal Tax { get; set; }
    }
}
