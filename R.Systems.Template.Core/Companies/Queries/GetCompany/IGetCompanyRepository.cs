using R.Systems.Template.Core.Common.DataTransferObjects;

namespace R.Systems.Template.Core.Companies.Queries.GetCompany;

public interface IGetCompanyRepository
{
    Task<CompanyDto?> GetCompanyAsync(int companyId);
}
