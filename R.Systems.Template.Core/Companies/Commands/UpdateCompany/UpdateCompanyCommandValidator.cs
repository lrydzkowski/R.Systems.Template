using FluentValidation;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

internal class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        Transform(c => c.Name, x => x?.Trim()).NotEmpty().MaximumLength(200);
    }
}
