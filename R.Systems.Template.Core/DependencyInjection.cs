using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Validation;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

namespace R.Systems.Template.Core;

public static class DependencyInjection
{
    public static void ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        services.AddAutoMapper(typeof(DependencyInjection));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.AddValidators();
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

    private static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateCompanyCommand>, CreateCompanyCommandValidator>();
        services.AddScoped<IValidator<UpdateCompanyCommand>, UpdateCompanyCommandValidator>();
        services.AddScoped<IValidator<CreateEmployeeCommand>, CreateEmployeeCommandValidator>();
        services.AddScoped<IValidator<UpdateEmployeeCommand>, UpdateEmployeeCommandValidator>();
    }
}
