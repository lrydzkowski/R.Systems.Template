using System.Net;
using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Items;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Services;

namespace R.Systems.Template.Infrastructure.CosmosDb.Companies.Queries;

internal class GetCompanyRepository : IGetCompanyRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly ICompanyMapper _companyMapper;

    public GetCompanyRepository(AppDbContext appDbContext, ICompanyMapper companyMapper)
    {
        _appDbContext = appDbContext;
        _companyMapper = companyMapper;
    }

    public string Version { get; } = Versions.V4;

    public async Task<Company?> GetCompanyAsync(long companyId, CancellationToken cancellationToken)
    {
        using ResponseMessage responseMessage = await _appDbContext.CompaniesContainer.ReadItemStreamAsync(
            companyId.ToString(),
            new PartitionKey(companyId.ToString()),
            cancellationToken: cancellationToken
        );
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        responseMessage.EnsureSuccessStatusCode();

        CosmosSystemTextJsonSerializer serializer = new();
        CompanyItem companyItem = serializer.FromStream<CompanyItem>(responseMessage.Content);

        Company company = _companyMapper.Map(companyItem);

        return company;
    }
}
