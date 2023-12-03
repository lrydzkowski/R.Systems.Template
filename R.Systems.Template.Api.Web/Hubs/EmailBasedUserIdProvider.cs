using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace R.Systems.Template.Api.Web.Hubs;

public class EmailBasedUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        string value = connection.User?.FindFirst(ClaimTypes.Upn)?.Value!;

        return value;
    }
}
