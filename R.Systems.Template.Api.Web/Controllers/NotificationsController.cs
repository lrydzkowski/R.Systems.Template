using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.Notifications.Models;
using R.Systems.Template.Infrastructure.Notifications.Services;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
public class NotificationsController : ApiControllerBase
{
    private const string WebSocketProtocol = "websocket";
    private readonly INotificationsRepository _notificationsRepository;
    private readonly IWebSocketsHandler _webSocketsHandler;

    public NotificationsController(
        IWebSocketsHandler webSocketsHandler,
        INotificationsRepository notificationsRepository
    )
    {
        _webSocketsHandler = webSocketsHandler;
        _notificationsRepository = notificationsRepository;
    }

    [Route("notifications")]
    public async Task Initiate()
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            return;
        }

        using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync(WebSocketProtocol);

        TaskCompletionSource<object> socketFinishedTcs = new();
        _webSocketsHandler.AddWebSocketInfo(new WebSocketInfo(webSocket, socketFinishedTcs, "test@gmail.com"));
        List<string> notifications = _notificationsRepository.GetNotifications();
        await _webSocketsHandler.SendMessageAsync(new NotificationsMessage { Notifications = notifications });

        await socketFinishedTcs.Task;
    }
}
