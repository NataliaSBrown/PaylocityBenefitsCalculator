namespace Api.Configurations
{
    public class PayrollConfig
    {
        public decimal BaseCost { get; set; }
        public decimal DependentCost { get; set; }
        public decimal AdditionalDependentCost { get; set; }
        public int HighSalaryThreshold { get; set; }
        public decimal HighSalaryExtraCostPercent { get; set; }
        public int PaychecksPerYear { get; set; }
        public int SeniorAgeThreshold { get; set; }
    }
}
