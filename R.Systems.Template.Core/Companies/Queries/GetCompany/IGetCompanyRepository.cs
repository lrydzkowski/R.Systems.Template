using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Companies.Queries.GetCompany;

public interface IGetCompanyRepository : IVersionedRepository
{
    Task<Company?> GetCompanyAsync(long companyId, CancellationToken cancellationToken);
}
