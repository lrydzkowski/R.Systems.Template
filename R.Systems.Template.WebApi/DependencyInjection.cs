using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using R.Systems.Template.WebApi.Swagger;
using System.Reflection;

namespace R.Systems.Template.WebApi;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.ConfigureControllers();
        services.AddEndpointsApiExplorer();
        services.ConfigureSwagger();
        services.ConfigureCors();
        services.ConfigureVersioning();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    private static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(
            config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }
        );
    }

    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            }
        );
    }

    private static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
    }
}
