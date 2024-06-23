using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.AzureFunctions.Services;

namespace R.Systems.Template.Api.AzureFunctions;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpResponseBuilder, HttpResponseBuilder>();
        services.AddSingleton<CustomJsonSerializer>();
        services.AddSingleton<IRequestPayloadSerializer, RequestPayloadSerializer>();
    }
}
