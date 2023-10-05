using Api.Configurations;
using Api.Dtos.Employee;

namespace Api.Services
{
    public interface IBenefitCalculator
    {
        decimal CalculateBenefit(GetEmployeeDto employee, PayrollConfig config);
    }

    public class BaseBenefitCalculator : IBenefitCalculator
    {
        public decimal CalculateBenefit(GetEmployeeDto employee, PayrollConfig config)
        {
            return config.BaseCost + (employee.Dependents.Count * config.DependentCost);
        }
    }

    public class HighSalaryBenefitCalculator : IBenefitCalculator
    {
        public decimal CalculateBenefit(GetEmployeeDto employee, PayrollConfig config)
        {
            return (employee.Salary > config.HighSalaryThreshold) ? employee.Salary * config.HighSalaryExtraCostPercent : 0;
        }
    }

    public class DependentAgeSeniorThresholdBenefitCalculator : IBenefitCalculator
    {
        public decimal CalculateBenefit(GetEmployeeDto employee, PayrollConfig config)
        {
            return employee.Dependents.Count(d => IsSenior(d.DateOfBirth, config.SeniorAgeThreshold)) * config.AdditionalDependentCost;
        }

        private static bool IsSenior(DateTime dateOfBirth, int seniorAgeThreshold)
        {
            return dateOfBirth.AddYears(seniorAgeThreshold) <= DateTime.Today;
        }
    }
}
