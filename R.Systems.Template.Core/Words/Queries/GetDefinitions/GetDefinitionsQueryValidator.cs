using FluentValidation;

namespace R.Systems.Template.Core.Words.Queries.GetDefinitions;

public class GetDefinitionsQueryValidator : AbstractValidator<GetDefinitionsQuery>
{
    public GetDefinitionsQueryValidator()
    {
        RuleFor(x => x.Word).NotEmpty().MaximumLength(200);
    }
}
