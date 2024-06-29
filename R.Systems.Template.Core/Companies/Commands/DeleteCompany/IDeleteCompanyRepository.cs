using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Companies.Commands.DeleteCompany;

public interface IDeleteCompanyRepository : IVersionedRepository
{
    Task DeleteAsync(long companyId);
}
