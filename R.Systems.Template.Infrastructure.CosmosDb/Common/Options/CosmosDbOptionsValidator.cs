using FluentValidation;

namespace R.Systems.Template.Infrastructure.CosmosDb.Common.Options;

internal class CosmosDbOptionsValidator : AbstractValidator<CosmosDbOptions>
{
    public CosmosDbOptionsValidator()
    {
        DefineAccountUriValidator();
        DefineDatabaseNameValidator();
    }

    private void DefineAccountUriValidator()
    {
        RuleFor(x => x.AccountUri)
            .NotEmpty()
            .WithName(nameof(CosmosDbOptions.AccountUri))
            .OverridePropertyName($"{CosmosDbOptions.Position}.{nameof(CosmosDbOptions.AccountUri)}");
    }

    private void DefineDatabaseNameValidator()
    {
        RuleFor(x => x.DatabaseName)
            .NotEmpty()
            .WithName(nameof(CosmosDbOptions.DatabaseName))
            .OverridePropertyName($"{CosmosDbOptions.Position}.{nameof(CosmosDbOptions.DatabaseName)}");
    }
}
