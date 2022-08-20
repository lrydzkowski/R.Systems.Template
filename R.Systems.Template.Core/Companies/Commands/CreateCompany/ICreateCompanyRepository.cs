using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

public interface ICreateCompanyRepository
{
    Task<Company> CreateCompanyAsync(CompanyToCreate company);
}
