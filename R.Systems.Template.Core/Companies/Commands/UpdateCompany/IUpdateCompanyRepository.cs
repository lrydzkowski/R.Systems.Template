using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public interface IUpdateCompanyRepository : IVersionedRepository
{
    Task<Company> UpdateCompanyAsync(CompanyToUpdate companyToUpdate);
}
