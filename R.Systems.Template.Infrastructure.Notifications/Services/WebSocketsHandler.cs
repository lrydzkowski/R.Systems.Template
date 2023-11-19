using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using R.Systems.Template.Infrastructure.Notifications.Models;

namespace R.Systems.Template.Infrastructure.Notifications.Services;

public interface IWebSocketsHandler
{
    void AddWebSocketInfo(WebSocketInfo socket);
    Task SendMessageAsync(NotificationsMessage message);
}

internal class WebSocketsHandler : IWebSocketsHandler
{
    private List<WebSocketInfo> _sockets = new();

    public void AddWebSocketInfo(WebSocketInfo socket)
    {
        _sockets.Add(socket);
    }

    public async Task SendMessageAsync(NotificationsMessage message)
    {
        ArraySegment<byte> serializedMessageArraySegments = SerializeMessage(message);

        _sockets = _sockets.Where(x => x.WebSocket.State == WebSocketState.Open).ToList();
        foreach (WebSocketInfo socket in _sockets)
        {
            await socket.WebSocket.SendAsync(
                serializedMessageArraySegments,
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }
    }

    private static ArraySegment<byte> SerializeMessage(NotificationsMessage message)
    {
        string serializedMessage = JsonSerializer.Serialize(message);
        byte[] serializedMessageBytes = Encoding.UTF8.GetBytes(serializedMessage);
        ArraySegment<byte> serializedMessageArraySegments = new(serializedMessageBytes);

        return serializedMessageArraySegments;
    }
}
