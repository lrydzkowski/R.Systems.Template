using MongoDB.Driver;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Lists.Extensions;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;

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
        List<string> fieldsAvailableToSort = [nameof(CompanyDocument.Id), nameof(CompanyDocument.Name)];
        List<string> fieldsAvailableToFilter = [nameof(CompanyDocument.Name)];
        List<Company> companies = _appDbContext.Companies.AsQueryable()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, nameof(CompanyDocument.Id))
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Paginate(listParameters.Pagination)
            .Select(document => new Company { CompanyId = document.Id!, Name = document.Name })
            .ToList();
        int count = _appDbContext.Companies.AsQueryable()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, nameof(CompanyDocument.Id))
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Select(document => document.Id!)
            .Count();

        return Task.FromResult(
            new ListInfo<Company>
            {
                Data = companies,
                Count = count
            }
        );
    }
}
