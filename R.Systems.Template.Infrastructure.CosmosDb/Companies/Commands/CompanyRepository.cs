using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Items;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.CosmosDb.Companies.Commands;

internal class CompanyRepository : ICreateCompanyRepository, IUpdateCompanyRepository, IDeleteCompanyRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly ICompanyMapper _companyMapper;
    private readonly IGetCompanyRepository _getCompanyRepository;

    public CompanyRepository(
        AppDbContext appDbContext,
        ICompanyMapper companyMapper,
        IGetCompanyRepository getCompanyRepository
    )
    {
        _appDbContext = appDbContext;
        _companyMapper = companyMapper;
        _getCompanyRepository = getCompanyRepository;
    }

    public string Version { get; } = Versions.V4;

    public async Task<Company> CreateCompanyAsync(CompanyToCreate companyToCreate)
    {
        CompanyItem companyItem = _companyMapper.Map(companyToCreate);
        CompanyItem createdCompanyItem = await _appDbContext.CompaniesContainer.CreateItemAsync(
            companyItem,
            new PartitionKey(companyItem.Id)
        );

        Company createdCompany = _companyMapper.Map(createdCompanyItem);

        return createdCompany;
    }

    public async Task DeleteCompanyAsync(long companyId)
    {
        Company? company = await _getCompanyRepository.GetCompanyAsync(companyId, CancellationToken.None);
        if (company is null)
        {
            return;
        }

        await _appDbContext.CompaniesContainer.DeleteItemAsync<Company>(
            companyId.ToString(),
            new PartitionKey(companyId.ToString())
        );
    }

    public async Task<Company> UpdateCompanyAsync(CompanyToUpdate companyToUpdate)
    {
        CompanyItem companyItem = _companyMapper.Map(companyToUpdate);
        CompanyItem updatedCompanyItem = await _appDbContext.CompaniesContainer.UpsertItemAsync(
            companyItem,
            new PartitionKey(companyItem.Id)
        );

        Company updatedCompany = _companyMapper.Map(updatedCompanyItem);

        return updatedCompany;
    }
}
