using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

internal interface IUpdateCompanyRepository
{
    Task UpdateCompanyAsync(Company company);
}
