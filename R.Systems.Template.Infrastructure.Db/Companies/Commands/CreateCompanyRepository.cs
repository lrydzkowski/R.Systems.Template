using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Infrastructure.Db.Common.Entities;
using R.Systems.Template.Infrastructure.Db.Common.Mappers;

namespace R.Systems.Template.Infrastructure.Db.Companies.Commands;

internal class CreateCompanyRepository : ICreateCompanyRepository
{
    public CreateCompanyRepository(AppDbContext dbContext, DbExceptionHandler dbExceptionHandler)
    {
        DbContext = dbContext;
        DbExceptionHandler = dbExceptionHandler;
    }

    private AppDbContext DbContext { get; }
    private DbExceptionHandler DbExceptionHandler { get; }

    public async Task<Company> CreateCompanyAsync(CompanyToCreate companyToCreate)
    {
        CompanyEntityMapper mapper = new();
        CompanyEntity companyEntity = mapper.ToCompanyEntity(companyToCreate);

        await DbContext.Companies.AddAsync(companyEntity);

        try
        {
            await DbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandler.Handle(exception, companyEntity);
            throw;
        }

        return mapper.ToCompany(companyEntity);
    }
}
