using NLog;
using NLog.Web;
using R.Systems.Template.Api.Web.Middleware;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.Db;
using R.Systems.Template.Infrastructure.Wordnik;

namespace R.Systems.Template.Api.Web;

public class Program
{
    public static void Main(string[] args)
    {
        Logger? logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("init main");

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
            logger.Error(exception, "Stopped program because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
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
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
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
