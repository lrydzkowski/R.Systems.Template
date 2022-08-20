using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Queries.GetCompanies;

public interface IGetCompaniesRepository
{
    Task<List<Company>> GetCompaniesAsync();
}
