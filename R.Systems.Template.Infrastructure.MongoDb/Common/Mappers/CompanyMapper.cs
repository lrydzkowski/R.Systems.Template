using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;
using R.Systems.Template.Infrastructure.MongoDb.Common.Services;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Mappers;

internal interface ICompanyMapper
{
    CompanyDocument Map(CompanyToCreate companyToCreate);
    List<Company> Map(List<CompanyDocument> companyDocuments);
    Company Map(CompanyDocument companyDocument);
}

internal class CompanyMapper
    : ICompanyMapper
{
    private readonly IUniqueIdGenerator _uniqueIdGenerator;

    public CompanyMapper(IUniqueIdGenerator uniqueIdGenerator)
    {
        _uniqueIdGenerator = uniqueIdGenerator;
    }

    public CompanyDocument Map(CompanyToCreate companyToCreate)
    {
        CompanyDocument companyDocument = new()
        {
            Id = _uniqueIdGenerator.Generate(),
            Name = companyToCreate.Name
        };

        return companyDocument;
    }

    public List<Company> Map(List<CompanyDocument> companyDocuments)
    {
        return companyDocuments.Select(Map).ToList();
    }

    public Company Map(CompanyDocument companyDocument)
    {
        Company company = new()
        {
            CompanyId = companyDocument.Id,
            Name = companyDocument.Name
        };

        return company;
    }
}
