using System.Net.WebSockets;

namespace R.Systems.Template.Infrastructure.Notifications.Models;

public record WebSocketInfo(WebSocket WebSocket, TaskCompletionSource<object> SocketFinishedTcs, string UserEmail);
