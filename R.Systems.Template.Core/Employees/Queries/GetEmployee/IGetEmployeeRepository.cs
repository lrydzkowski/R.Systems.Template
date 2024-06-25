using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployee;

public interface IGetEmployeeRepository : IVersionedRepository
{
    Task<Employee?> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken);

    Task<Employee?> GetEmployeeAsync(int companyId, int employeeId, CancellationToken cancellationToken);
}
