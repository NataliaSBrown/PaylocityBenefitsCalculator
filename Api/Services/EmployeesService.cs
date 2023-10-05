using Api.Data;
using Api.Dtos.Employee;
using AutoMapper;

namespace Api.Services
{
    public interface IEmployeesService
    {
        Task<GetEmployeeDto> GetEmployeeByIdAsync(int id);
        Task<List<GetEmployeeDto>> GetEmployeesAsync();
    }

    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeeDataRepository _employeeDataRepository;
        private readonly IMapper _mapper;

        public EmployeesService(IEmployeeDataRepository employeeDataRepository, IMapper mapper)
        {
            _employeeDataRepository = employeeDataRepository ?? throw new ArgumentNullException(nameof(employeeDataRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        }

        public async Task<GetEmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employees = await _employeeDataRepository.GetEmployeesWithCacheAsync();
            var employee = employees.FirstOrDefault(e => e.Id == id);

            return _mapper.Map<GetEmployeeDto>(employee);
        }

        public async Task<List<GetEmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _employeeDataRepository.GetEmployeesWithCacheAsync();

            return _mapper.Map<List<GetEmployeeDto>>(employees);
        }
    }
}
