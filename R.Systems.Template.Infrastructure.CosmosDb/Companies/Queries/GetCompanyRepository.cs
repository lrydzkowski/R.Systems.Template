using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Queries.GetCompany;

namespace R.Systems.Template.Infrastructure.CosmosDb.Companies.Queries;

internal class GetCompanyRepository : IGetCompanyRepository
{
    private readonly CosmosClient _cosmosClient;

    public GetCompanyRepository(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    public string Version { get; } = Versions.V4;

    public Task<Company?> GetCompanyAsync(long companyId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
