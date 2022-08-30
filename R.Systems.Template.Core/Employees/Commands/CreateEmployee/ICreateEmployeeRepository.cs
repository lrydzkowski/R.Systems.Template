using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

public interface ICreateEmployeeRepository
{
    Task<Employee> CreateEmployeeAsync(EmployeeToCreate employeeToCreate);
}
