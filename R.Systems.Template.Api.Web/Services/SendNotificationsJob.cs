using Quartz;
using R.Systems.Template.Infrastructure.Notifications.Models;
using R.Systems.Template.Infrastructure.Notifications.Services;

namespace R.Systems.Template.Api.Web.Services;

public class SendNotificationsJob : IJob
{
    private readonly ILogger<SendNotificationsJob> _logger;
    private readonly INotificationsRepository _notificationsRepository;
    private readonly IWebSocketsHandler _webSocketsHandler;

    public SendNotificationsJob(
        ILogger<SendNotificationsJob> logger,
        INotificationsRepository notificationsRepository,
        IWebSocketsHandler webSocketsHandler
    )
    {
        _logger = logger;
        _notificationsRepository = notificationsRepository;
        _webSocketsHandler = webSocketsHandler;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("It works - great :)");

        List<string> message = _notificationsRepository.GetNotifications();
        await _webSocketsHandler.SendMessageAsync(new NotificationsMessage { Notifications = message });
    }
}
