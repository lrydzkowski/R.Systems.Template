using FluentValidation;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

internal class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
