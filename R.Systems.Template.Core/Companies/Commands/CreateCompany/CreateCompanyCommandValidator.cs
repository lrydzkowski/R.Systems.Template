using FluentValidation;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

internal class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
