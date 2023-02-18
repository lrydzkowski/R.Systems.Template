using FluentValidation;

namespace R.Systems.Template.Infrastructure.Azure.Options;

internal class AzureAdB2COptionsValidator : AbstractValidator<AzureAdB2COptions>
{
    public AzureAdB2COptionsValidator()
    {
        DefineInstanceValidator();
        DefineClientIdValidator();
        DefineDomainValidator();
        DefineSignUpSignInPolicyId();
    }

    private void DefineInstanceValidator()
    {
        RuleFor(x => x.Instance)
            .NotEmpty()
            .WithName(nameof(AzureAdB2COptions.Instance))
            .OverridePropertyName($"{AzureAdB2COptions.Position}.{nameof(AzureAdB2COptions.Instance)}");
    }

    private void DefineClientIdValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithName(nameof(AzureAdB2COptions.ClientId))
            .OverridePropertyName($"{AzureAdB2COptions.Position}.{nameof(AzureAdB2COptions.ClientId)}");
    }

    private void DefineDomainValidator()
    {
        RuleFor(x => x.Domain)
            .NotEmpty()
            .WithName(nameof(AzureAdB2COptions.Domain))
            .OverridePropertyName($"{AzureAdB2COptions.Position}.{nameof(AzureAdB2COptions.Domain)}");
    }

    private void DefineSignUpSignInPolicyId()
    {
        RuleFor(x => x.SignUpSignInPolicyId)
            .NotEmpty()
            .WithName(nameof(AzureAdB2COptions.SignUpSignInPolicyId))
            .OverridePropertyName($"{AzureAdB2COptions.Position}.{nameof(AzureAdB2COptions.SignUpSignInPolicyId)}");
    }
}
