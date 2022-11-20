using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db;
using R.Systems.Template.Tests.Integration.Common.Authentication;
using R.Systems.Template.Tests.Integration.Common.Db;
using R.Systems.Template.WebApi;
using RestSharp;

namespace R.Systems.Template.Tests.Integration.Common.WebApplication;

internal static class WebApiFactoryBuilder
{
    public static RestClient CreateRestClient(this WebApplicationFactory<Program> webApplicationFactory)
    {
        return new RestClient(webApplicationFactory.CreateClient());
    }

    public static WebApplicationFactory<Program> WithoutData(
        this WebApplicationFactory<Program> webApplicationFactory
    )
    {
        return webApplicationFactory.WithWebHostBuilder(
            builder =>
            {
                builder.ConfigureServices(
                    services =>
                    {
                        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
                        AppDbContext appDbContext = (AppDbContext)scope.ServiceProvider.GetRequiredService(
                            typeof(AppDbContext)
                        );

                        DbInitializer.RemoveExistingData(appDbContext);
                    }
                );
            }
        );
    }

    public static WebApplicationFactory<Program> WithoutAuthentication(
        this WebApplicationFactory<Program> webApplicationFactory
    )
    {
        return webApplicationFactory.WithWebHostBuilder(
            builder => builder.ConfigureServices(
                services => services.AddSingleton<IAuthorizationHandler, AllowAnonymous>()
            )
        );
    }

    public static WebApplicationFactory<Program> WithCustomOptions(
        this WebApplicationFactory<Program> webApplicationFactory,
        Dictionary<string, string?> customOptions
    )
    {
        return webApplicationFactory.WithWebHostBuilder(
            builder => builder.ConfigureAppConfiguration(
                (_, configBuilder) => configBuilder.AddInMemoryCollection(customOptions)
            )
        );
    }
}
