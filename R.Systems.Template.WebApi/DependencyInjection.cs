using Microsoft.OpenApi.Models;

namespace R.Systems.Template.WebApi;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.ConfigureSwagger();
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "R.Systems.Template.WebApi", Version = "1.0" });
                options.EnableAnnotations();
            }
        );
    }
}
