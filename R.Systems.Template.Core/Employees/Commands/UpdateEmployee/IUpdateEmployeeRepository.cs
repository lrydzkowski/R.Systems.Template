using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public interface IUpdateEmployeeRepository
{
    Task<Employee> UpdateEmployeeAsync(EmployeeToUpdate employeeToUpdate);
}
