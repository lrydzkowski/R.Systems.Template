using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

public interface ICreateEmployeeRepository : IVersionedRepository
{
    Task<Employee> CreateEmployeeAsync(EmployeeToCreate employeeToCreate);
}
