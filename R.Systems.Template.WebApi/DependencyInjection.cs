using Microsoft.OpenApi.Models;
using R.Systems.Template.Persistence.Db;
using RunMethodsSequentially;

namespace R.Systems.Template.WebApi;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.ConfigureSwagger();
        services.ConfigureCors();
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.ConfigureSequentialServices(environment);
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

    private static void ConfigureSequentialServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.RegisterRunMethodsSequentially(
                options => options.AddFileSystemLockAndRunMethods(environment.ContentRootPath)
            )
            .RegisterServiceToRunInJob<AppDbInitializer>();
    }
}
