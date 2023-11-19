using R.Systems.Template.Infrastructure.Notifications.Models;
using R.Systems.Template.Infrastructure.Notifications.Services;
using System.Net.WebSockets;

namespace R.Systems.Template.Api.Web.Middleware;

public class NotificationsWebSocketsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebSocketsHandler _webSocketsHandler;

    public NotificationsWebSocketsMiddleware(
        RequestDelegate next,
        IWebSocketsHandler webSocketsHandler
    )
    {
        _next = next;
        _webSocketsHandler = webSocketsHandler;
    }

    public async Task InvokeAsync(HttpContext context, INotificationsRepository notificationsRepository)
    {
        if (context.Request.Path != "/notifications")
        {
            await _next(context);

            return;
        }

        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return;
        }

        using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        TaskCompletionSource<object> socketFinishedTcs = new();

        _webSocketsHandler.AddWebSocketInfo(new WebSocketInfo(webSocket, socketFinishedTcs, "test@gmail.com"));
        List<string> notifications = notificationsRepository.GetNotifications();
        await _webSocketsHandler.SendMessageAsync(new NotificationsMessage { Notifications = notifications });

        await socketFinishedTcs.Task;
    }
}

public static class NotificationsWebSocketsMiddlewareExtensions
{
    public static IApplicationBuilder UseNotificationsWebSockets(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<NotificationsWebSocketsMiddleware>();
    }
}
