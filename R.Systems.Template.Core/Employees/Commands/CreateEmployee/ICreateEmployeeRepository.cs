using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

public interface ICreateEmployeeRepository
{
    Task<Result<Employee>> CreateEmployeeAsync(EmployeeToCreate employeeToCreate);
}
