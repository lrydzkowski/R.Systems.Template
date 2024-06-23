using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
    private readonly string? _name;
    private readonly IValidator<TOptions> _validator;

    public FluentValidationOptions(string? name, IValidator<TOptions> validator)
    {
        _name = name;
        _validator = validator;
    }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (_name != null && _name != name)
        {
            return ValidateOptionsResult.Skip;
        }

        ArgumentNullException.ThrowIfNull(options);
        ValidationResult validationResult = _validator.Validate(options);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("App settings -", validationResult.Errors, true);
        }

        return ValidateOptionsResult.Success;
    }
}
