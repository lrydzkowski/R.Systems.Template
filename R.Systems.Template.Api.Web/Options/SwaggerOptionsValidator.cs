using FluentValidation;

namespace R.Systems.Template.Api.Web.Options;

public class SwaggerOptionsValidator : AbstractValidator<SwaggerOptions>
{
    public SwaggerOptionsValidator()
    {
        DefinePasswordValidator();
    }

    private void DefinePasswordValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithName(nameof(SwaggerOptions.Password))
            .OverridePropertyName($"{SwaggerOptions.Position}.{nameof(SwaggerOptions.Password)}");
    }
}
