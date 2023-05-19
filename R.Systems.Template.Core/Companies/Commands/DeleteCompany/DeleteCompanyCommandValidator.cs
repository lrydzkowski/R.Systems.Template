using FluentValidation;

namespace R.Systems.Template.Core.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty().WithName(nameof(DeleteCompanyCommand.CompanyId));
    }
}
