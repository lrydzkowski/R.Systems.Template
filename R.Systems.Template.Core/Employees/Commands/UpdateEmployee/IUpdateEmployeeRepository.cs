using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

public interface IUpdateEmployeeRepository : IVersionedRepository
{
    Task<Employee> UpdateEmployeeAsync(EmployeeToUpdate employeeToUpdate);
}
