namespace R.Systems.Template.Infrastructure.Notifications.Models;

public record NotificationsMessage
{
    public List<string> Notifications { get; init; } = new();
}
