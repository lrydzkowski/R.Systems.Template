using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

[Mapper]
internal partial class CreateEmployeeCommandMapper
{
    public partial EmployeeToCreate ToEmployeeToCreate(CreateEmployeeCommand command);
}
