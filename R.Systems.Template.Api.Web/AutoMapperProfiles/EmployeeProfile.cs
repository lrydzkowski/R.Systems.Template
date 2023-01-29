using AutoMapper;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

namespace R.Systems.Template.Api.Web.AutoMapperProfiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<CreateEmployeeRequest, CreateEmployeeCommand>();
        CreateMap<UpdateEmployeeRequest, UpdateEmployeeCommand>()
            .ForMember(dest => dest.EmployeeId, opts => opts.Ignore());
    }
}
