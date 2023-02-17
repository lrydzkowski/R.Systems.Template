using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core;

public static class DependencyInjection
{
    public static void ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddMediatR();
        services.AddAutoMapper(typeof(DependencyInjection));
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
    }

    public static void ConfigureOptionsWithValidation<TOptions, TValidator>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationPosition
    )
        where TOptions : class
        where TValidator : class, IValidator<TOptions>, new()
    {
        services.AddSingleton<IValidator<TOptions>, TValidator>();
        services.AddOptions<TOptions>()
            .Bind(configuration.GetSection(configurationPosition))
            .Validate(
                config =>
                {
                    ServiceProvider serviceProvider = services.BuildServiceProvider();
                    using IServiceScope scope = serviceProvider.CreateScope();
                    IValidator<TOptions>? validator = (IValidator<TOptions>?)scope.ServiceProvider.GetService(
                        typeof(IValidator<TOptions>)
                    );
                    if (validator == null)
                    {
                        return true;
                    }

                    ValidationResult validationResult = validator.Validate(config);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(
                            "App settings -",
                            validationResult.Errors,
                            appendDefaultMessage: true
                        );
                    }

                    return true;
                }
            )
            .ValidateOnStart();
    }

    private static void AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}
