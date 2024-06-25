using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;

namespace R.Systems.Template.Infrastructure.MongoDb.Companies.Queries;

internal class GetCompaniesRepository : IGetCompaniesRepository
{
    public string Version { get; } = Versions.V2;

    public Task<ListInfo<Company>> GetCompaniesAsync(ListParameters listParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
