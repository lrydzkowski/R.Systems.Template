using FluentValidation;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class CreateCompanyValidator : AbstractValidator<CompanyToCreate>
{
    public CreateCompanyValidator(AppDbContext dbContext)
    {
        DbContext = dbContext;
        DefineNameValidators();
    }

    private AppDbContext DbContext { get; }

    private void DefineNameValidators()
    {
        RuleFor(x => x.Name)
            .MustAsync(ValidateCompanyNameUniquenessAsync)
            .WithName("Name")
            .WithMessage("Company with the given name already exists ('{PropertyValue}').")
            .WithErrorCode("Exists");
    }

    private async Task<bool> ValidateCompanyNameUniquenessAsync(
        string name,
        CancellationToken cancellationToken
    )
    {
        int? foundCompanyId = await DbContext.Companies.AsNoTracking()
            .Where(companyEntity => companyEntity.Name == name)
            .Select(companyEntity => companyEntity.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return foundCompanyId == null;
    }
}
