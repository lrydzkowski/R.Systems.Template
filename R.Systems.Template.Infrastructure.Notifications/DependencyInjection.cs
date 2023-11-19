using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Infrastructure.Notifications.Services;

namespace R.Systems.Template.Infrastructure.Notifications;

public static class DependencyInjection
{
    public static void ConfigureNotificationsServices(this IServiceCollection services)
    {
        services.ConfigureServices();
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IWebSocketsHandler, WebSocketsHandler>();
        services.AddScoped<INotificationsRepository, NotificationsRepository>();
    }
}
