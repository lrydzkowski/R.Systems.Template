using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace R.Systems.Template.Api.Web.Hubs;

public class EmailBasedUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        string value = connection.User?.FindFirst(ClaimTypes.Upn)?.Value!;
        return value;
    }
}
