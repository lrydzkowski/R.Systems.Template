using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class UpdateCompanyRepository : IUpdateCompanyRepository
{
    public UpdateCompanyRepository(IMapper mapper, AppDbContext dbContext, DbExceptionHandler dbExceptionHandler)
    {
        Mapper = mapper;
        DbContext = dbContext;
        DbExceptionHandler = dbExceptionHandler;
    }

    private IMapper Mapper { get; }
    private AppDbContext DbContext { get; }
    private DbExceptionHandler DbExceptionHandler { get; }

    public async Task<Result<Company>> UpdateCompanyAsync(CompanyToUpdate companyToUpdate)
    {
        Result<CompanyEntity> getCompanyEntityResult = await GetCompanyEntityAsync(companyToUpdate.CompanyId);
        if (getCompanyEntityResult.IsFaulted)
        {
            return getCompanyEntityResult.MapFaulted<Company>();
        }

        CompanyEntity companyEntity = getCompanyEntityResult.Value!;
        companyEntity.Name = companyToUpdate.Name;

        try
        {
            await DbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            List<ValidationFailure> validationFailures = DbExceptionHandler.Handle(exception, companyEntity);
            if (validationFailures.Count == 0)
            {
                throw;
            }

            return new Result<Company>(new ValidationException(validationFailures));
        }

        return Mapper.Map<Company>(companyEntity);
    }

    private async Task<Result<CompanyEntity>> GetCompanyEntityAsync(int companyId)
    {
        CompanyEntity? companyEntity = await DbContext.Companies.Where(x => x.Id == companyId)
            .FirstOrDefaultAsync();
        if (companyEntity == null)
        {
            ValidationException validationException = new(
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

            return new Result<CompanyEntity>(validationException);
        }

        return companyEntity;
    }
}
