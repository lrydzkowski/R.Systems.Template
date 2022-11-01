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
        services.ConfigureOptionsWithValidation<AzureAdOptions, AzureAdOptionsValidator>(
            configuration,
            AzureAdOptions.Position
        );
        services.AddMicrosoftIdentityWebApiAuthentication(configuration);
    }
}
