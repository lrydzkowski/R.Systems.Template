using R.Systems.Template.Infrastructure.Notifications.Models;

namespace R.Systems.Template.Api.Web.Hubs;

public interface INotificationsClient
{
    Task ReceiveNewNotifications(NotificationsMessage message);
}
