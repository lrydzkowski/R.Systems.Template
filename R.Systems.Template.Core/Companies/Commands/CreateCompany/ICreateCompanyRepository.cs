using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

public interface ICreateCompanyRepository : IVersionedRepository
{
    Task<Company> CreateCompanyAsync(CompanyToCreate company);
}
