using AutoMapper;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

internal class UpdateEmployeeCommandProfile : Profile
{
    public UpdateEmployeeCommandProfile()
    {
        CreateMap<UpdateEmployeeCommand, EmployeeToUpdate>();
    }
}
