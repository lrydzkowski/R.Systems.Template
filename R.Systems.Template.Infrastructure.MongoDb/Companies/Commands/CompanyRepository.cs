using MongoDB.Driver;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;
using R.Systems.Template.Infrastructure.MongoDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.MongoDb.Companies.Commands;

internal class CompanyRepository : ICreateCompanyRepository, IUpdateCompanyRepository, IDeleteCompanyRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly ICompanyMapper _companyMapper;

    public CompanyRepository(
        AppDbContext appDbContext,
        ICompanyMapper companyMapper
    )
    {
        _appDbContext = appDbContext;
        _companyMapper = companyMapper;
    }

    public string Version { get; } = Versions.V2;

    public async Task<Company> CreateCompanyAsync(CompanyToCreate company)
    {
        CompanyDocument companyDocument = _companyMapper.Map(company);
        await _appDbContext.Companies.InsertOneAsync(companyDocument);
        Company createdCompany = _companyMapper.Map(companyDocument);

        return createdCompany;
    }

    public async Task DeleteAsync(long companyId)
    {
        FilterDefinition<CompanyDocument>? filter = Builders<CompanyDocument>.Filter.Eq(d => d.Id, companyId);
        await _appDbContext.Companies.DeleteOneAsync(filter);
    }

    public async Task<Company> UpdateCompanyAsync(CompanyToUpdate companyToUpdate)
    {
        FilterDefinition<CompanyDocument> filter =
            Builders<CompanyDocument>.Filter.Where(x => x.Id == companyToUpdate.CompanyId);
        UpdateDefinition<CompanyDocument> updateDefinition =
            Builders<CompanyDocument>.Update.Set(x => x.Name, companyToUpdate.Name);

        CompanyDocument updatedDocument = await _appDbContext.Companies.FindOneAndUpdateAsync(
            filter,
            updateDefinition,
            new FindOneAndUpdateOptions<CompanyDocument>
            {
                ReturnDocument = ReturnDocument.After
            }
        );
        Company updatedCompany = _companyMapper.Map(updatedDocument);

        return updatedCompany;
    }
}
