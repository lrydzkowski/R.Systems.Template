using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Employees.Commands.DeleteEmployee;

public interface IDeleteEmployeeRepository : IVersionedRepository
{
    Task DeleteEmployeeAsync(int employeeId);
}
