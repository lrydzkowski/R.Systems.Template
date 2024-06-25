using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Infrastructure.Db.Common.Entities;
using CompanyEntityMapper = R.Systems.Template.Infrastructure.Db.Common.Mappers.CompanyEntityMapper;

namespace R.Systems.Template.Infrastructure.Db.Companies.Commands;

internal class CompanyRepository : ICreateCompanyRepository, IUpdateCompanyRepository, IDeleteCompanyRepository
{
    private readonly AppDbContext _dbContext;
    private readonly DbExceptionHandler _dbExceptionHandler;

    public CompanyRepository(AppDbContext dbContext, DbExceptionHandler dbExceptionHandler)
    {
        _dbContext = dbContext;
        _dbExceptionHandler = dbExceptionHandler;
    }

    public string Version { get; } = Versions.V1;

    public async Task<Company> CreateCompanyAsync(CompanyToCreate companyToCreate)
    {
        CompanyEntityMapper mapper = new();
        CompanyEntity companyEntity = mapper.ToCompanyEntity(companyToCreate);
        await _dbContext.Companies.AddAsync(companyEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            _dbExceptionHandler.Handle(exception, companyEntity);
            throw;
        }

        return mapper.ToCompany(companyEntity);
    }

    public async Task DeleteAsync(int companyId)
    {
        CompanyEntity company = await GetCompanyEntityAsync(companyId);
        _dbContext.Companies.Remove(company);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Company> UpdateCompanyAsync(CompanyToUpdate companyToUpdate)
    {
        CompanyEntityMapper mapper = new();
        CompanyEntity companyEntity = await GetCompanyEntityAsync(companyToUpdate.CompanyId);
        companyEntity.Name = companyToUpdate.Name;
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            _dbExceptionHandler.Handle(exception, companyEntity);
            throw;
        }

        return mapper.ToCompany(companyEntity);
    }

    private async Task<CompanyEntity> GetCompanyEntityAsync(int companyId)
    {
        CompanyEntity? companyEntity = await _dbContext.Companies.Where(x => x.Id == companyId).FirstOrDefaultAsync();
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
