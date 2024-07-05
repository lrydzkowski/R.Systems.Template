using MassTransit;
using MongoDB.Driver;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;

namespace R.Systems.Template.Infrastructure.MongoDb.Companies.Queries;

internal class GetCompaniesRepository : IGetCompaniesRepository
{
    private readonly AppDbContext _appDbContext;

    public GetCompaniesRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public string Version { get; } = Versions.V2;

    public Task<ListInfo<Company>> GetCompaniesAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Company> query = _appDbContext.Companies.AsQueryable()
            .Select(companyEntity => new Company { CompanyId = companyEntity.Id!, Name = companyEntity.Name })
            .Sort(listParameters.Sorting, listParameters.Fields)
            .Filter(listParameters.Filters, listParameters.Fields);

        List<Company> companies = query
            .Paginate(listParameters.Pagination)
            .ToList()
            .AsQueryable()
            .Project(listParameters.Fields)
            .ToList();
        int count = query.Count();

        return Task.FromResult(
            new ListInfo<Company>
            {
                Data = companies,
                Count = count
            }
        );
    }
}
