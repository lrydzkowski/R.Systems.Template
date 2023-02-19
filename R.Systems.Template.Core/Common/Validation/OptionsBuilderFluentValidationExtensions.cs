using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace R.Systems.Template.Core.Common.Validation;

internal static class OptionsBuilderFluentValidationExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder)
        where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
            serviceProvider => new FluentValidationOptions<TOptions>(
                optionsBuilder.Name,
                serviceProvider.GetRequiredService<IValidator<TOptions>>()
            )
        );

        return optionsBuilder;
    }
}

internal class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    public FluentValidationOptions(string? name, IValidator<TOptions> validator)
    {
        Name = name;
        Validator = validator;
    }

    private string? Name { get; }
    private IValidator<TOptions> Validator { get; }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (Name != null && Name != name)
        {
            return ValidateOptionsResult.Skip;
        }

        ArgumentNullException.ThrowIfNull(options);

        ValidationResult validationResult = Validator.Validate(options);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(
                "App settings -",
                validationResult.Errors,
                appendDefaultMessage: true
            );
        }

        return ValidateOptionsResult.Success;
    }
}
