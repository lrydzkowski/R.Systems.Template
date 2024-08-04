using FluentValidation;

namespace R.Systems.Template.Infrastructure.Azure.Options;

internal class AzureAdOptionsValidator : AbstractValidator<AzureAdOptions>
{
    public AzureAdOptionsValidator()
    {
        DefineInstanceValidator();
        DefineClientIdValidator();
        DefineClientSecretValidator();
        DefineTenantIdValidator();
        DefineAudienceValidator();
    }

    private void DefineInstanceValidator()
    {
        RuleFor(x => x.Instance)
            .NotEmpty()
            .WithName(nameof(AzureAdOptions.Instance))
            .OverridePropertyName($"{AzureAdOptions.Position}.{nameof(AzureAdOptions.Instance)}");
    }

    private void DefineClientIdValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithName(nameof(AzureAdOptions.ClientId))
            .OverridePropertyName($"{AzureAdOptions.Position}.{nameof(AzureAdOptions.ClientId)}");
    }

    private void DefineClientSecretValidator()
    {
        RuleFor(x => x.ClientSecret)
            .NotEmpty()
            .WithName(nameof(AzureAdOptions.ClientSecret))
            .OverridePropertyName($"{AzureAdOptions.Position}.{nameof(AzureAdOptions.ClientSecret)}");
    }

    private void DefineTenantIdValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty()
            .WithName(nameof(AzureAdOptions.TenantId))
            .OverridePropertyName($"{AzureAdOptions.Position}.{nameof(AzureAdOptions.TenantId)}");
    }

    private void DefineAudienceValidator()
    {
        RuleFor(x => x.Audience)
            .NotEmpty()
            .WithName(nameof(AzureAdOptions.Audience))
            .OverridePropertyName($"{AzureAdOptions.Position}.{nameof(AzureAdOptions.Audience)}");
    }
}
