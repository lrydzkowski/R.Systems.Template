using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using R.Systems.Template.Api.Web.Auth;
using R.Systems.Template.Api.Web.Hubs;
using R.Systems.Template.Api.Web.Middleware;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.Db;
using R.Systems.Template.Infrastructure.Notifications;
using R.Systems.Template.Infrastructure.Wordnik;

namespace R.Systems.Template.Api.Web;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        WebApplication app = builder.Build();
        ConfigureRequestPipeline(app);
        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.ConfigureServices(builder.Configuration, builder.Environment);
        builder.Services.ConfigureCoreServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureDbServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureAzureServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureWordnikServices(builder.Configuration);
        builder.Services.ConfigureNotificationsServices();
    }

    private static void ConfigureRequestPipeline(WebApplication app)
    {
        app.UseHttpLogging();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSwagger();
        app.UseCors(DependencyInjection.CorsPolicy);
        app.UseSwaggerUI();

        UseHealthChecks(app);
        app.UseWebSocketsAuth();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHub<NotificationsHub>(NotificationsHub.Path);
        app.UseWebSockets();
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
