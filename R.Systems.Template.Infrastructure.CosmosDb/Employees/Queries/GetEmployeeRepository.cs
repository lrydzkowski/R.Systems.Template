using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.CosmosDb.Employees.Queries;

internal class GetEmployeeRepository : IGetEmployeeRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly IEmployeeMapper _employeeMapper;

    public GetEmployeeRepository(CosmosClient cosmosClient, IEmployeeMapper employeeMapper)
    {
        _cosmosClient = cosmosClient;
        _employeeMapper = employeeMapper;
    }

    public string Version { get; } = Versions.V4;

    public Task<Employee?> GetEmployeeAsync(long employeeId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Employee?> GetEmployeeAsync(long companyId, long employeeId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
