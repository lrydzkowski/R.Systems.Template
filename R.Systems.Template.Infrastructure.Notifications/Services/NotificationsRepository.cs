namespace R.Systems.Template.Infrastructure.Notifications.Services;

public interface INotificationsRepository
{
    List<string> GetNotifications();
}

internal class NotificationsRepository : INotificationsRepository
{
    private readonly Random _random = new();

    public List<string> GetNotifications()
    {
        List<string> notifications = new();
        int notificationsCount = _random.Next(1, 10);
        for (int i = 0; i < notificationsCount; i++)
        {
            notifications.Add(Guid.NewGuid().ToString());
        }

        return notifications;
    }
}
