using FluentValidation;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty().WithName(nameof(UpdateCompanyCommand.CompanyId));
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
