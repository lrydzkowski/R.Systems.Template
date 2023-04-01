using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Infrastructure.Db.Common.Entities;
using R.Systems.Template.Infrastructure.Db.Common.Mappers;

namespace R.Systems.Template.Infrastructure.Db.Companies.Commands;

internal class UpdateCompanyRepository : IUpdateCompanyRepository
{
    public UpdateCompanyRepository(AppDbContext dbContext, DbExceptionHandler dbExceptionHandler)
    {
        DbContext = dbContext;
        DbExceptionHandler = dbExceptionHandler;
    }

    private AppDbContext DbContext { get; }
    private DbExceptionHandler DbExceptionHandler { get; }

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
