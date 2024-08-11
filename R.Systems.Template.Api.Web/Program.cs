using R.Systems.Template.Api.Web.Hubs;
using R.Systems.Template.Api.Web.Middleware;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.CosmosDb;
using R.Systems.Template.Infrastructure.MongoDb;
using R.Systems.Template.Infrastructure.Notifications;
using R.Systems.Template.Infrastructure.PostgreSqlDb;
using R.Systems.Template.Infrastructure.SqlServerDb;
using R.Systems.Template.Infrastructure.Wordnik;
using Serilog;
using Serilog.Debugging;

namespace R.Systems.Template.Api.Web;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = Serilog.CreateBootstrapLogger();
        SelfLog.Enable(Console.Error);
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
        builder.Services.ConfigureCoreServices(builder.Configuration);
        builder.Services.ConfigureInfrastructurePostgreSqlDbServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureSqlServerDbServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureMongoDbServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureAzureServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureWordnikServices(builder.Configuration);
        builder.Services.ConfigureNotificationsServices();
        builder.Services.ConfigureInfrastructureCosmosDbServices(builder.Configuration);
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);
        builder.Host.UseSerilog(Serilog.CreateLogger, true);
    }

    private static void ConfigureRequestPipeline(WebApplication app)
    {
        app.UseHttpLogging();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSwagger();
        app.UseCors(DependencyInjection.CorsPolicy);
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI();
        }

        app.UseWebSocketsAuth();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHub<NotificationsHub>(NotificationsHub.Path);
        app.UseWebSockets();
    }
}
