using AutoMapper;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

internal class CreateEmployeeCommandProfile : Profile
{
    public CreateEmployeeCommandProfile()
    {
        CreateMap<CreateEmployeeCommand, EmployeeToCreate>();
    }
}
