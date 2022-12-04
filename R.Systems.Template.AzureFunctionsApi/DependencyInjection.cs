using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.AzureFunctionsApi.Services;

namespace R.Systems.Template.AzureFunctionsApi;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddSingleton<HttpResponseBuilder>();
        services.AddSingleton<CustomJsonSerializer>();
    }
}
