using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
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
            .AddMicrosoftIdentityWebApi(configuration, Constants.AzureAd, AuthenticationSchemes.AzureAd);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration, Constants.AzureAdB2C, AuthenticationSchemes.AzureAdB2C);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(
                options =>
                {
                    IConfigurationSection configurationSection = configuration.GetSection(Constants.AzureAd);
                    configurationSection.Bind(options);
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            StringValues accessToken = context.Request.Query["access_token"];
                            PathString path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                },
                options =>
                {
                    IConfigurationSection configurationSection = configuration.GetSection(Constants.AzureAd);
                    configurationSection.Bind(options);
                },
                AuthenticationSchemes.AzureAdForSignalR
            );
    }
}
