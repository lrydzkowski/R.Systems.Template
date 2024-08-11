using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.CosmosDb.Companies.Commands;

internal class CompanyRepository : ICreateCompanyRepository, IUpdateCompanyRepository, IDeleteCompanyRepository
{
    private readonly ICompanyMapper _companyMapper;
    private readonly CosmosClient _cosmosClient;

    public CompanyRepository(CosmosClient cosmosClient, ICompanyMapper companyMapper)
    {
        _cosmosClient = cosmosClient;
        _companyMapper = companyMapper;
    }

    public string Version { get; } = Versions.V4;

    public Task<Company> CreateCompanyAsync(CompanyToCreate company)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCompanyAsync(long companyId)
    {
        throw new NotImplementedException();
    }

    public Task<Company> UpdateCompanyAsync(CompanyToUpdate companyToUpdate)
    {
        throw new NotImplementedException();
    }
}
