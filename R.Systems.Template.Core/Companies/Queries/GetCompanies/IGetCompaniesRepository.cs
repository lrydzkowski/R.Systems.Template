using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Companies.Queries.GetCompanies;

public interface IGetCompaniesRepository
{
    Task<ListInfo<Company>>
        GetCompaniesAsync(ListParameters listParameters, CancellationToken cancellationToken);
}
