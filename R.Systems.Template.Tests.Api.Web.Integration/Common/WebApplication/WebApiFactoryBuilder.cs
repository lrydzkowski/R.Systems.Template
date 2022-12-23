using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.Web;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Authentication;
using RestSharp;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;

internal static class WebApiFactoryBuilder
{
    public static RestClient CreateRestClient(this WebApplicationFactory<Program> webApplicationFactory)
    {
        return new RestClient(webApplicationFactory.CreateClient());
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
