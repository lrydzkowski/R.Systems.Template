using FluentValidation;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class CompanyValidator : AbstractValidator<CompanyEntity>
{
    public CompanyValidator(ValidateCompanyRepository validateCompanyRepository)
    {
        DefineNameValidators(validateCompanyRepository);
    }

    private void DefineNameValidators(ValidateCompanyRepository validateCompanyRepository)
    {
        RuleFor(x => x.Name)
            .MustAsync(async (name, _) =>
            {
                bool companyExists = await validateCompanyRepository.CompanyNameExists(name);

                return !companyExists;
            })
            .WithMessage("Company with '{PropertyName}' = '{PropertyValue}' exists.")
            .WithErrorCode("Exists");
    }
}
