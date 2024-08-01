using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

[Mapper]
internal partial class UpdateEmployeeCommandMapper
{
    public partial EmployeeToUpdate ToEmployeeToUpdate(UpdateEmployeeCommand command);

    public Guid Map(string value)
    {
        return Guid.Parse(value);
    }
}
