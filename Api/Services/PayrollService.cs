using Api.Configurations;
using Api.Dtos.Employee;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly PayrollConfig _config;

        public PayrollService(IEnumerable<IBenefitCalculator> benefitCalculators, IMapper mapper, IOptions<PayrollConfig> config)
        {
            _benefitCalculators = benefitCalculators ?? throw new ArgumentNullException(nameof(benefitCalculators));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public GetEmployeePaycheckDto CalculatePaycheck(GetEmployeeDto employee)
        {
            decimal paycheckDeductions = CalculatePaycheckDeductions(employee);
            decimal paycheckTotal = CalculatePaycheckTotal(employee.Salary, paycheckDeductions);

            return CreateEmployeePaycheckDto(employee, paycheckTotal);
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

        private GetEmployeePaycheckDto CreateEmployeePaycheckDto(GetEmployeeDto employee, decimal paycheckTotal)
        {
            var employeePaycheck = _mapper.Map<GetEmployeePaycheckDto>(employee);
            employeePaycheck.Paycheck = paycheckTotal;
            return employeePaycheck;
        }
    }
}
