using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;

namespace R.Systems.Template.Infrastructure.MongoDb.Companies.Commands;

internal class CompanyRepository : ICreateCompanyRepository, IUpdateCompanyRepository, IDeleteCompanyRepository
{
    public string Version { get; } = Versions.V2;

    public Task<Company> CreateCompanyAsync(CompanyToCreate company)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public Task<Company> UpdateCompanyAsync(CompanyToUpdate companyToUpdate)
    {
        throw new NotImplementedException();
    }
}
