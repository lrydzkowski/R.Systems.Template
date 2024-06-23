using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;

namespace R.Systems.Template.Infrastructure.Db.Companies.Queries;

internal class GetCompaniesRepository : IGetCompaniesRepository
{
    private readonly AppDbContext _dbContext;

    public GetCompaniesRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ListInfo<Company>> GetCompaniesAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        List<string> fieldsAvailableToSort = new()
        {
            "id",
            "name"
        };
        List<string> fieldsAvailableToFilter = new()
        {
            "name"
        };
        List<Company> companies = await _dbContext.Companies.AsNoTracking()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, "id")
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Paginate(listParameters.Pagination)
            .Select(companyEntity => new Company { CompanyId = (int)companyEntity.Id!, Name = companyEntity.Name })
            .ToListAsync(cancellationToken);
        int count = await _dbContext.Companies.AsNoTracking()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, "id")
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Select(companyEntity => (int)companyEntity.Id!)
            .CountAsync(cancellationToken);
        return new ListInfo<Company>
        {
            Data = companies,
            Count = count
        };
    }
}
