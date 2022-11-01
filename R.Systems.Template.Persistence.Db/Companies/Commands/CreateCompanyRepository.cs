using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;
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

    public async Task<Result<Company>> CreateCompanyAsync(CompanyToCreate companyToCreate)
    {
        CompanyEntity companyEntity = Mapper.Map<CompanyEntity>(companyToCreate);

        await DbContext.Companies.AddAsync(companyEntity);

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
}
