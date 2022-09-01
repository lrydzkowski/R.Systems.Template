using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db;
using R.Systems.Template.Tests.Integration.Common.Authentication;
using R.Systems.Template.Tests.Integration.Common.Db;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;
using RestSharp;

namespace R.Systems.Template.Tests.Integration.Common.Builders;

internal static class RestClientBuilder
{
    public static RestClient CreateRestClient(this WebApiFactory<Program> webApiFactory)
    {
        return new RestClient(webApiFactory.CreateClient());
    }

    public static RestClient CreateRestClientWithoutData(this WebApiFactory<Program> webApiFactory)
    {
        HttpClient httpClient = webApiFactory.WithWebHostBuilder(
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
        ).CreateClient();

        return new RestClient(httpClient);
    }

    public static RestClient CreateRestClientWithoutAuthentication(this WebApiFactory<Program> webApiFactory)
    {
        HttpClient httpClient = webApiFactory.WithWebHostBuilder(
            builder => builder.ConfigureServices(
                services =>
                {
                    services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
                }
            )
        ).CreateClient();

        return new RestClient(httpClient);
    }
}
