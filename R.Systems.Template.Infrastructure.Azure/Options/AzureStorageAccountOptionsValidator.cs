using FluentValidation;

namespace R.Systems.Template.Infrastructure.Azure.Options;

internal class AzureStorageAccountOptionsValidator : AbstractValidator<AzureStorageAccountOptions>
{
    public AzureStorageAccountOptionsValidator()
    {
        DefineNameValidator();
    }

    private void DefineNameValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithName(nameof(AzureStorageAccountOptions.Name))
            .OverridePropertyName($"{AzureStorageAccountOptions.Position}.{nameof(AzureStorageAccountOptions.Name)}");
    }
}
