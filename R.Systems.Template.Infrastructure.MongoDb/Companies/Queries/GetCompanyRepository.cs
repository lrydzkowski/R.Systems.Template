using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Queries.GetCompany;

namespace R.Systems.Template.Infrastructure.MongoDb.Companies.Queries;

internal class GetCompanyRepository : IGetCompanyRepository
{
    public string Version { get; } = Versions.V2;

    public Task<Company?> GetCompanyAsync(int companyId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
