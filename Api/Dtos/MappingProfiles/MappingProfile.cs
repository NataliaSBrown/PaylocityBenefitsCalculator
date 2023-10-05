using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using AutoMapper;

namespace Api.Dtos.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Employee, GetEmployeeDto>();
            CreateMap<Models.Dependent, GetDependentDto>();
            CreateMap<GetEmployeeDto, GetEmployeePaycheckDto>()
                .ForMember(e => e.Paycheck, opt => opt.Ignore());
        }
    }
}
