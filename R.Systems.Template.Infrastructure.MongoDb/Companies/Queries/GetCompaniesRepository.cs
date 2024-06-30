using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;
using R.Systems.Template.Infrastructure.MongoDb.Common.Extensions;
using R.Systems.Template.Infrastructure.MongoDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.MongoDb.Companies.Queries;

internal class GetCompaniesRepository : IGetCompaniesRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly ICompanyMapper _companyMapper;

    public GetCompaniesRepository(AppDbContext appDbContext, ICompanyMapper companyMapper)
    {
        _appDbContext = appDbContext;
        _companyMapper = companyMapper;
    }

    public string Version { get; } = Versions.V2;

    public async Task<ListInfo<Company>> GetCompaniesAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<string> fieldsAvailableToFilter = [nameof(CompanyDocument.Name)];
        IReadOnlyList<string> fieldsAvailableToSort = [nameof(CompanyDocument.Id), nameof(CompanyDocument.Name)];
        string defaultSortingFieldName = nameof(CompanyDocument.Id);
        ListInfo<CompanyDocument> result = await _appDbContext.Companies.GetDataAsync(
            listParameters,
            fieldsAvailableToFilter,
            fieldsAvailableToSort,
            defaultSortingFieldName,
            cancellationToken: cancellationToken
        );

        return new ListInfo<Company>
        {
            Count = result.Count,
            Data = _companyMapper.Map(result.Data)
        };
    }
}
