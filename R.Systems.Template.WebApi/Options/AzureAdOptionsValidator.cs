using FluentValidation;

namespace R.Systems.Template.WebApi.Options;

internal class AzureAdOptionsValidator : AbstractValidator<AzureAdOptions>
{
    public AzureAdOptionsValidator()
    {
        RuleFor(x => x.Instance).NotEmpty()
            .WithName("Instance")
            .OverridePropertyName($"{AzureAdOptions.Position}.Instance");
        RuleFor(x => x.ClientId).NotEmpty()
            .WithName("ClientId")
            .OverridePropertyName($"{AzureAdOptions.Position}.ClientId");
        RuleFor(x => x.Domain).NotEmpty()
            .WithName("Domain")
            .OverridePropertyName($"{AzureAdOptions.Position}.Domain");
        RuleFor(x => x.SignUpSignInPolicyId).NotEmpty()
            .WithName("SignUpSignInPolicyId")
            .OverridePropertyName($"{AzureAdOptions.Position}.SignUpSignInPolicyId");
    }
}
