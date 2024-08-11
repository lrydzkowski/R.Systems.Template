using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;

namespace R.Systems.Template.Infrastructure.CosmosDb.Companies.Queries;

internal class GetCompaniesRepository : IGetCompaniesRepository
{
    private readonly AppDbContext _appDbContext;

    public GetCompaniesRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public string Version { get; } = Versions.V4;

    public Task<ListInfo<Company>> GetCompaniesAsync(ListParameters listParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
