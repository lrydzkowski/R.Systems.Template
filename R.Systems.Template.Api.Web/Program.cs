using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using R.Systems.Template.Api.Web.Auth;
using R.Systems.Template.Api.Web.Middleware;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.Db;
using R.Systems.Template.Infrastructure.Wordnik;
using Serilog;

namespace R.Systems.Template.Api.Web;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = Serilog.CreateBootstrapLogger();

        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            ConfigureLogging(builder);
            WebApplication app = builder.Build();
            ConfigureRequestPipeline(app);
            app.Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Application terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.ConfigureServices(builder.Configuration, builder.Environment);
        builder.Services.ConfigureCoreServices();
        builder.Services.ConfigureInfrastructureDbServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureAzureServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureWordnikServices(builder.Configuration);
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);
        builder.Host.UseSerilog(Serilog.CreateLogger, true);
    }

    private static void ConfigureRequestPipeline(WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSwagger();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI();
        }

        UseHealthChecks(app);
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }

    private static void UseHealthChecks(WebApplication app)
    {
        app.MapHealthChecks(
                "/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }
            )
            .RequireAuthorization(
                builder => builder.AddAuthenticationSchemes(ApiKeyAuthenticationSchemeOptions.Name)
                    .RequireAuthenticatedUser()
            );
    }
}
