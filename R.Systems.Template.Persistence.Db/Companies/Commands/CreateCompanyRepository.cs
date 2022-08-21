using AutoMapper;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class CreateCompanyRepository : ICreateCompanyRepository
{
    public CreateCompanyRepository(IMapper mapper, AppDbContext dbContext, DbExceptionHandler dbExceptionHandler)
    {
        Mapper = mapper;
        DbContext = dbContext;
        DbExceptionHandler = dbExceptionHandler;
    }

    private IMapper Mapper { get; }
    private AppDbContext DbContext { get; }
    private DbExceptionHandler DbExceptionHandler { get; }

    public async Task<Company> CreateCompanyAsync(CompanyToCreate companyToCreate)
    {
        CompanyEntity companyEntity = Mapper.Map<CompanyEntity>(companyToCreate);

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

        return Mapper.Map<Company>(companyEntity);
    }
}
