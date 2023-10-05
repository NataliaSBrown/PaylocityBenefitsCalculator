using Api.Configurations;
using Api.Dtos.Employee;
using Microsoft.Extensions.Options;

namespace Api.Services
{
    public interface IPayrollService
    {
        GetEmployeePaycheckDto CalculatePaycheck(GetEmployeeDto employee);
    }

    public class PayrollService : IPayrollService
    {
        private readonly IEnumerable<IBenefitCalculator> _benefitCalculators;
        private readonly PayrollConfig _config;

        public PayrollService(IEnumerable<IBenefitCalculator> benefitCalculators, IOptions<PayrollConfig> config)
        {
            _benefitCalculators = benefitCalculators ?? throw new ArgumentNullException(nameof(benefitCalculators));
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public GetEmployeePaycheckDto CalculatePaycheck(GetEmployeeDto employee)
        {
            decimal paycheckDeductions = CalculatePaycheckDeductions(employee);
            decimal paycheckTotal = CalculatePaycheckTotal(employee.Salary, paycheckDeductions);

            return MapToPaycheckDto(employee, paycheckTotal);
        }

        private decimal CalculatePaycheckDeductions(GetEmployeeDto employee)
        {
            return _benefitCalculators.Sum(calculator => calculator.CalculateBenefit(employee, _config));
        }

        private decimal CalculatePaycheckTotal(decimal salary, decimal deductions)
        {
            decimal paycheckTotal = (salary - deductions) / _config.PaychecksPerYear;
            return Math.Round(paycheckTotal, 2, MidpointRounding.ToZero);
        }

        private static GetEmployeePaycheckDto MapToPaycheckDto(GetEmployeeDto employee, decimal paycheck)
        {
            return new GetEmployeePaycheckDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,                
                Paycheck = paycheck
            };
        }
    }
}
