using Microsoft.OpenApi.Models;

namespace R.Systems.Template.WebApi.Swagger;

public static class ServiceExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "R.Systems.Template.WebApi", Version = "1.0" });
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "R.Systems.Template.WebApi", Version = "2.0" });
                options.EnableAnnotations();
            }
        );
    }
}
