using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Azure.Options;

namespace R.Systems.Template.Infrastructure.Azure;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureAzureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureOptions(configuration);
        services.ConfigureAuthentication(configuration);
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<AzureAdOptions, AzureAdOptionsValidator>(
            configuration,
            AzureAdOptions.Position
        );
        services.ConfigureOptionsWithValidation<AzureAdB2COptions, AzureAdB2COptionsValidator>(
            configuration,
            AzureAdB2COptions.Position
        );
    }

    private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration, "AzureAd", JwtBearerDefaults.AuthenticationScheme);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration, "AzureAdB2C", AuthenticationSchemes.AzureAdB2C);
    }
}
