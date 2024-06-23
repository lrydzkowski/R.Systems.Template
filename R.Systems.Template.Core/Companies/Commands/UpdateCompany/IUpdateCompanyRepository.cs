using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public interface IUpdateCompanyRepository
{
    Task<Company> UpdateCompanyAsync(CompanyToUpdate companyToUpdate);
}
