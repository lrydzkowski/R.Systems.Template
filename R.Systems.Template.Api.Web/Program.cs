using Microsoft.ApplicationInsights.Extensibility;
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
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/startup-.log", rollingInterval: RollingInterval.Day)
            .CreateBootstrapLogger();

        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            ConfigureSettings(builder);
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

    private static void ConfigureSettings(WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile(
            $"serilog.{builder.Environment.EnvironmentName}.json",
            optional: true,
            reloadOnChange: true
        );
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.ConfigureServices(builder.Environment);
        builder.Services.ConfigureCoreServices();
        builder.Services.ConfigureInfrastructureDbServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureAzureServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureWordnikServices(builder.Configuration);
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);
        builder.Host.UseSerilog(
            (context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .WriteTo.ApplicationInsights(
                    services.GetRequiredService<TelemetryConfiguration>(),
                    TelemetryConverter.Traces
                )
                .WriteTo.AzureBlobStorage(
                    connectionStringName: "StorageAccount",
                    context.Configuration,
                    storageFileName: "r-systems-template-api-logs/{yyyy}-{MM}-{dd}.log"
                )
        );
    }

    private static void ConfigureRequestPipeline(WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSwagger();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI();
        }

        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}
