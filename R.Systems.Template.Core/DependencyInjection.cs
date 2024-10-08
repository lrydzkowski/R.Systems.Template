using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Services;
using R.Systems.Template.Core.Common.Validation;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Core.Jobs;
using R.Systems.Template.Core.Words.Queries.GetDefinitions;

namespace R.Systems.Template.Core;

public static class DependencyInjection
{
    public static void ConfigureCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMediatR();
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.ConfigureMassTransit(configuration);
        services.RegisterServices();
    }

    public static void ConfigureOptionsWithValidation<TOptions, TValidator>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationPosition
    )
        where TOptions : class where TValidator : class, IValidator<TOptions>, new()
    {
        services.AddSingleton<IValidator<TOptions>, TValidator>();
        services.ConfigureOptions<TOptions>(configuration, configurationPosition);
    }

    public static void ConfigureOptions<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationPosition
    )
        where TOptions : class
    {
        services.AddOptions<TOptions>()
            .Bind(configuration.GetSection(configurationPosition))
            .ValidateFluently()
            .ValidateOnStart();
    }

    private static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.AddRequestPreProcessor<CreateCompanyCommandPreProcessor>();
                cfg.AddRequestPreProcessor<UpdateCompanyCommandPreProcessor>();
                cfg.AddRequestPreProcessor<CreateEmployeeCommandPreProcessor>();
                cfg.AddRequestPreProcessor<UpdateEmployeeCommandPreProcessor>();
                cfg.AddRequestPreProcessor<GetDefinitionsQueryPreProcessor>();
            }
        );
    }

    private static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        if (EnvHandler.IsSystemUnderTest(configuration))
        {
            return;
        }

        services.AddMassTransit(
            x =>
            {
                x.AddConsumer<LogInformationConsumer>();
                x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
                x.ConfigureHealthCheckOptions(options => options.Name = "MassTransitCheck");
            }
        );
    }

    private static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IVersionedRepositoryFactory<>), typeof(VersionedRepositoryFactory<>));
        services.AddScoped<IListParametersMapper, ListParametersMapper>();
        services.AddScoped<IUniqueIdGenerator, UniqueIdGenerator>();
    }
}
