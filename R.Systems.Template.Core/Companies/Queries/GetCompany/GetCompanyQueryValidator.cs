using FluentValidation;

namespace R.Systems.Template.Core.Companies.Queries.GetCompany;

public class GetCompanyQueryValidator : AbstractValidator<GetCompanyQuery>
{
    public GetCompanyQueryValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty().WithName(nameof(GetCompanyQuery.CompanyId));
    }
}
