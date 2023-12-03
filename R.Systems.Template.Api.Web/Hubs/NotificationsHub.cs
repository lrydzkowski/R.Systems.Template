using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.Notifications.Models;
using R.Systems.Template.Infrastructure.Notifications.Services;

namespace R.Systems.Template.Api.Web.Hubs;

[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAdForSignalR)]
public class NotificationsHub : Hub<INotificationsClient>
{
    public const string Path = "/notifications-hub";

    private readonly INotificationsRepository _notificationsRepository;

    public NotificationsHub(INotificationsRepository notificationsRepository)
    {
        _notificationsRepository = notificationsRepository;
    }

    public NotificationsMessage GetAllNotifications()
    {
        List<string> notifications = _notificationsRepository.GetNotifications();
        NotificationsMessage message = new() { Notifications = notifications };

        return message;
    }
}
