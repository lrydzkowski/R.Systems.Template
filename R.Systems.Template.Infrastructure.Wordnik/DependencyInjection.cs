using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core;
using R.Systems.Template.Core.Words.Queries.GetDefinitions;
using R.Systems.Template.Infrastructure.Wordnik.Common.Api;
using R.Systems.Template.Infrastructure.Wordnik.Common.Options;
using R.Systems.Template.Infrastructure.Wordnik.Words.Queries.GetDefinitions;

namespace R.Systems.Template.Infrastructure.Wordnik;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureWordnikServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAutoMapper(typeof(DependencyInjection)).ConfigureOptions(configuration).ConfigureServices();
    }

    private static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<WordnikOptions, WordnikOptionsValidator>(
            configuration,
            WordnikOptions.Position
        );

        return services;
    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<WordApi>();
        services.AddSingleton<IGetDefinitionsRepository, GetDefinitionsRepository>();

        return services;
    }
}
