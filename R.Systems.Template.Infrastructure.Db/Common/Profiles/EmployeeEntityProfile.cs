using AutoMapper;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Infrastructure.Db.Common.Entities;

namespace R.Systems.Template.Infrastructure.Db.Common.Profiles;

internal class EmployeeEntityProfile : Profile
{
    public EmployeeEntityProfile()
    {
        CreateMap<EmployeeEntity, Employee>()
            .ForMember(employee => employee.EmployeeId, options => options.MapFrom(companyEntity => companyEntity.Id));
        CreateMap<EmployeeToCreate, EmployeeEntity>()
            .ForMember(employeeEntity => employeeEntity.Id, options => options.Ignore())
            .ForMember(employeeEntity => employeeEntity.Company, options => options.Ignore());
    }
}
