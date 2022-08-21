using FluentValidation;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class UpdateCompanyValidator : AbstractValidator<CompanyToUpdate>
{
    public UpdateCompanyValidator(AppDbContext dbContext)
    {
        DbContext = dbContext;
        DefineCompanyIdValidators();
        DefineNameValidators();
    }

    private AppDbContext DbContext { get; }

    private void DefineCompanyIdValidators()
    {
        RuleFor(x => x.CompanyId)
            .MustAsync(ValidateCompanyIdExistenceAsync)
            .WithName("CompanyId")
            .WithMessage("Company with the given company id doesn't exist ('{PropertyValue}').")
            .WithErrorCode("NotExist");
    }

    private async Task<bool> ValidateCompanyIdExistenceAsync(int companyId, CancellationToken cancellationToken)
    {
        return await DbContext.Companies.AsNoTracking()
                   .Where(companyEntity => companyEntity.Id == companyId)
                   .FirstOrDefaultAsync(cancellationToken)
               != null;
    }

    private void DefineNameValidators()
    {
        RuleFor(x => x.Name)
            .MustAsync(ValidateCompanyNameUniquenessAsync)
            .WithName("Name")
            .WithMessage("Company with the given name already exists ('{PropertyValue}').")
            .WithErrorCode("Exists");
    }

    private async Task<bool> ValidateCompanyNameUniquenessAsync(
        CompanyToUpdate companyToUpdate,
        string name,
        CancellationToken cancellationToken
    )
    {
        int? foundCompanyId = await DbContext.Companies.AsNoTracking()
            .Where(companyEntity => companyEntity.Id != companyToUpdate.CompanyId && companyEntity.Name == name)
            .Select(companyEntity => companyEntity.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return foundCompanyId == null;
    }
}
