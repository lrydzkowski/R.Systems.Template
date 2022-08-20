using FluentValidation;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

internal class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        Transform(c => c.Name, x => x?.Trim()).NotEmpty().MaximumLength(200);
    }
}
