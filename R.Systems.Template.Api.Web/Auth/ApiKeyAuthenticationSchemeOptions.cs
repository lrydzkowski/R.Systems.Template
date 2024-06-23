using Microsoft.AspNetCore.Authentication;

namespace R.Systems.Template.Api.Web.Auth;

public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string Name = "ApiKeyAuthenticationScheme";
}
