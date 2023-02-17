using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Queries.GetCompany;

public interface IGetCompanyRepository
{
    Task<Company?> GetCompanyAsync(int companyId, CancellationToken cancellationToken);
}
