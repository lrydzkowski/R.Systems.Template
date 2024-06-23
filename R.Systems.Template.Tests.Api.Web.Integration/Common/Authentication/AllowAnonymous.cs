using Microsoft.AspNetCore.Authorization;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Authentication;

internal class AllowAnonymous : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
