using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

public interface ICreateCompanyRepository
{
    Task<Result<Company>> CreateCompanyAsync(CompanyToCreate company);
}
