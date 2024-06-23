using Microsoft.AspNetCore.SignalR;
using Quartz;
using R.Systems.Template.Api.Web.Hubs;
using R.Systems.Template.Infrastructure.Notifications.Models;
using R.Systems.Template.Infrastructure.Notifications.Services;

namespace R.Systems.Template.Api.Web.Services;

public class SendNotificationsJob : IJob
{
    private readonly IHubContext<NotificationsHub, INotificationsClient> _hubContext;
    private readonly ILogger<SendNotificationsJob> _logger;
    private readonly INotificationsRepository _notificationsRepository;
    private readonly IWebSocketsHandler _webSocketsHandler;

    public SendNotificationsJob(
        ILogger<SendNotificationsJob> logger,
        INotificationsRepository notificationsRepository,
        IWebSocketsHandler webSocketsHandler,
        IHubContext<NotificationsHub, INotificationsClient> hubContext
    )
    {
        _logger = logger;
        _notificationsRepository = notificationsRepository;
        _webSocketsHandler = webSocketsHandler;
        _hubContext = hubContext;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("It works - great :)");
        List<string> notifications = _notificationsRepository.GetNotifications();
        NotificationsMessage message = new()
        {
            Notifications = notifications
        };
        await SendMessageByWebSocketAsync(message);
        await SendMessageBySignalRAsync(message);
    }

    private async Task SendMessageByWebSocketAsync(NotificationsMessage message)
    {
        await _webSocketsHandler.SendMessageAsync(message);
    }

    private async Task SendMessageBySignalRAsync(NotificationsMessage message)
    {
        await _hubContext.Clients.Users("admin1@lrspaceb2c.onmicrosoft.com").ReceiveNewNotifications(message);
    }
}
