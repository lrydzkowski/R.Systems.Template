using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public interface IUpdateCompanyRepository
{
    Task<Result<Company>> UpdateCompanyAsync(CompanyToUpdate companyToUpdate);
}
