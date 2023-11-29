using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace R.Systems.Template.Api.Web.Middleware;

/// <summary>
///     WebSockets authentication based on:
///     https://stackoverflow.com/questions/4361173/http-headers-in-websockets-client-api/77060459#77060459
/// </summary>
public class WebSocketsAuthMiddleware
{
    private readonly RequestDelegate _next;

    public WebSocketsAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/notifications")
        {
            context.Request.Headers.TryGetValue(HeaderNames.SecWebSocketProtocol, out StringValues foundValues);
            string[]? headerParts = foundValues.FirstOrDefault()?.Split(',');
            if (headerParts?.Length >= 2)
            {
                string authToken = headerParts[1].Trim();
                context.Request.Headers.TryAdd(HeaderNames.Authorization, $"Bearer {authToken}");
            }
        }

        await _next(context);
    }
}

public static class WebSocketsAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseWebSocketsAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<WebSocketsAuthMiddleware>();
    }
}
