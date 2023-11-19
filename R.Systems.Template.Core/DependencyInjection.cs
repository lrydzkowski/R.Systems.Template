using FluentValidation;
using MassTransit;
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
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.ConfigureMassTransit();
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
            .ValidateFluently()
            .ValidateOnStart();
    }

    private static void AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static void ConfigureMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(
            x =>
            {
                x.AddConsumer<LogInformationConsumer>();
                x.UsingInMemory(
                    (context, cfg) => cfg.ConfigureEndpoints(context)
                );
            }
        );
    }
}
