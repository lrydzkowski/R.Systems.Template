using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;

namespace R.Systems.Template.Infrastructure.CosmosDb.Employees.Queries;

internal class GetEmployeesRepository : IGetEmployeesRepository
{
    private readonly CosmosClient _cosmosClient;

    public GetEmployeesRepository(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    public string Version { get; } = Versions.V4;

    public Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        long companyId,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
