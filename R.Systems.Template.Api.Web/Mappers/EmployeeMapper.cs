using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Api.Web.Mappers;

[Mapper]
public partial class EmployeeMapper
{
    public partial CreateEmployeeCommand ToCreateCommand(CreateEmployeeRequest request);

    [MapperIgnoreTarget(nameof(UpdateEmployeeCommand.EmployeeId))]
    public partial UpdateEmployeeCommand ToUpdateCommand(UpdateEmployeeRequest request);
}
