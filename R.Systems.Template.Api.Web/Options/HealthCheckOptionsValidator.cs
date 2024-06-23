using FluentValidation;

namespace R.Systems.Template.Api.Web.Options;

public class HealthCheckOptionsValidator : AbstractValidator<HealthCheckOptions>
{
    public HealthCheckOptionsValidator()
    {
        DefineApiKeyValidator();
    }

    private void DefineApiKeyValidator()
    {
        RuleFor(x => x.ApiKey)
            .NotEmpty()
            .WithName(nameof(HealthCheckOptions.ApiKey))
            .OverridePropertyName($"{HealthCheckOptions.Position}.{nameof(HealthCheckOptions.ApiKey)}");
    }
}
