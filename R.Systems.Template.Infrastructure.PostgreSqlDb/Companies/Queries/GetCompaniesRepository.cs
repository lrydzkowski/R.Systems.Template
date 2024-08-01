using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Companies.Queries;

internal class GetCompaniesRepository : IGetCompaniesRepository
{
    private readonly AppDbContext _dbContext;

    public GetCompaniesRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Version { get; } = Versions.V1;

    public async Task<ListInfo<Company>> GetCompaniesAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Company> query = _dbContext.Companies.AsNoTracking()
            .Select(companyEntity => new Company { CompanyId = (Guid)companyEntity.Id!, Name = companyEntity.Name })
            .Sort(listParameters.Sorting, listParameters.Fields)
            .Filter(listParameters.Filters, listParameters.Fields);
        List<Company> companies = await query
            .Paginate(listParameters.Pagination)
            .Project(listParameters.Fields)
            .ToListAsync(cancellationToken);
        int count = await query.CountAsync(cancellationToken);

        return new ListInfo<Company>
        {
            Data = companies,
            Count = count
        };
    }
}
