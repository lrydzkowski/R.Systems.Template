using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public interface IUpdateEmployeeRepository
{
    Task<Result<Employee>> UpdateEmployeeAsync(EmployeeToUpdate employeeToUpdate);
}
