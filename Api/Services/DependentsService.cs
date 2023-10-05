using Api.Data;
using Api.Dtos.Dependent;
using AutoMapper;

namespace Api.Services
{
    public interface IDependentsService
    {
        Task<GetDependentDto> GetDependentByIdAsync(int id);
        Task<List<GetDependentDto>> GetDependentsAsync();
    }

    public class DependentsService : IDependentsService
    {
        private readonly IEmployeeDataRepository _employeeDataRepository;
        private readonly IMapper _mapper;

        public DependentsService(IEmployeeDataRepository employeeDataRepository, IMapper mapper)
        {
            _employeeDataRepository = employeeDataRepository ?? throw new ArgumentNullException(nameof(mapper)); ;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        }

        public async Task<GetDependentDto> GetDependentByIdAsync(int id)
        {
            var employees = await _employeeDataRepository.GetEmployeesWithCacheAsync();
            var dependent = employees.SelectMany(e => e.Dependents).FirstOrDefault(d => d.Id == id);

            return _mapper.Map<GetDependentDto>(dependent);
        }

        public async Task<List<GetDependentDto>> GetDependentsAsync()
        {
            var employees = await _employeeDataRepository.GetEmployeesWithCacheAsync();
            var dependents = employees.SelectMany(e => e.Dependents);

            return _mapper.Map<List<GetDependentDto>>(dependents);
        }
    }
}
