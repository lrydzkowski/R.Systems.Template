using FluentValidation;

namespace R.Systems.Template.Infrastructure.Wordnik.Common.Options;

internal class WordnikOptionsValidator : AbstractValidator<WordnikOptions>
{
    public WordnikOptionsValidator()
    {
        DefineApiBaseUrlValidator();
        DefineDefinitionsUrlValidator();
        DefineRandomWordUrlValidator();
        DefineApiKeyValidator();
    }

    private void DefineApiBaseUrlValidator()
    {
        RuleFor(x => x.ApiBaseUrl)
            .NotEmpty()
            .WithName(nameof(WordnikOptions.ApiBaseUrl))
            .OverridePropertyName($"{WordnikOptions.Position}.{nameof(WordnikOptions.ApiBaseUrl)}");
    }

    private void DefineDefinitionsUrlValidator()
    {
        RuleFor(x => x.DefinitionsUrl)
            .NotEmpty()
            .WithName(nameof(WordnikOptions.DefinitionsUrl))
            .OverridePropertyName($"{WordnikOptions.Position}.{nameof(WordnikOptions.DefinitionsUrl)}");
    }

    private void DefineRandomWordUrlValidator()
    {
        RuleFor(x => x.RandomWordUrl)
            .NotEmpty()
            .WithName(nameof(WordnikOptions.RandomWordUrl))
            .OverridePropertyName($"{WordnikOptions.Position}.{nameof(WordnikOptions.RandomWordUrl)}");
    }

    private void DefineApiKeyValidator()
    {
        RuleFor(x => x.ApiKey)
            .NotEmpty()
            .WithName(nameof(WordnikOptions.ApiKey))
            .OverridePropertyName($"{WordnikOptions.Position}.{nameof(WordnikOptions.ApiKey)}");
    }
}
