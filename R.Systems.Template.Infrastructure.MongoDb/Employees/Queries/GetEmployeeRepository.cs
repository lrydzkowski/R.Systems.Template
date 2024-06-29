using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;

namespace R.Systems.Template.Infrastructure.MongoDb.Employees.Queries;

internal class GetEmployeeRepository : IGetEmployeeRepository
{
    public string Version { get; } = Versions.V2;

    public Task<Employee?> GetEmployeeAsync(long employeeId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Employee?> GetEmployeeAsync(long companyId, long employeeId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
