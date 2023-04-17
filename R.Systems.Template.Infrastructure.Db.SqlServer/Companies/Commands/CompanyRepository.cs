using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Infrastructure.Db.SqlServer.Common.Entities;
using R.Systems.Template.Infrastructure.Db.SqlServer.Common.Mappers;

namespace R.Systems.Template.Infrastructure.Db.SqlServer.Companies.Commands;

internal class CompanyRepository : ICreateCompanyRepository, IUpdateCompanyRepository, IDeleteCompanyRepository
{
    public CompanyRepository(AppDbContext dbContext, DbExceptionHandler dbExceptionHandler)
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

    public async Task<Company> UpdateCompanyAsync(CompanyToUpdate companyToUpdate)
    {
        CompanyEntityMapper mapper = new();
        CompanyEntity companyEntity = await GetCompanyEntityAsync(companyToUpdate.CompanyId);
        companyEntity.Name = companyToUpdate.Name;

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

    public async Task DeleteAsync(int companyId)
    {
        CompanyEntity company = await GetCompanyEntityAsync(companyId);

        DbContext.Companies.Remove(company);
        await DbContext.SaveChangesAsync();
    }

    private async Task<CompanyEntity> GetCompanyEntityAsync(int companyId)
    {
        CompanyEntity? companyEntity = await DbContext.Companies.Where(x => x.Id == companyId)
            .FirstOrDefaultAsync();
        if (companyEntity == null)
        {
            throw new ValidationException(
                new List<ValidationFailure>
                {
                    new()
                    {
                        PropertyName = "Company",
                        ErrorMessage = $"Company with the given id doesn't exist ('{companyId}').",
                        AttemptedValue = companyId,
                        ErrorCode = "NotExist"
                    }
                }
            );
        }

        return companyEntity;
    }
}
