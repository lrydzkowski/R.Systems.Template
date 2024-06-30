using MongoDB.Driver;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;
using R.Systems.Template.Infrastructure.MongoDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.MongoDb.Companies.Queries;

internal class GetCompanyRepository : IGetCompanyRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly ICompanyMapper _companyMapper;

    public GetCompanyRepository(AppDbContext appDbContext, ICompanyMapper companyMapper)
    {
        _appDbContext = appDbContext;
        _companyMapper = companyMapper;
    }

    public string Version { get; } = Versions.V2;

    public async Task<Company?> GetCompanyAsync(long companyId, CancellationToken cancellationToken)
    {
        FilterDefinition<CompanyDocument>? filter = Builders<CompanyDocument>.Filter.Eq(x => x.Id, companyId);
        CompanyDocument? document = await _appDbContext.Companies.Find(filter).FirstOrDefaultAsync(cancellationToken);
        if (document == null)
        {
            return null;
        }

        Company company = _companyMapper.Map(document);

        return company;
    }
}
