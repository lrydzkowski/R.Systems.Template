using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using R.Systems.Template.Api.Web.Options;

namespace R.Systems.Template.Api.Web.Auth;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IOptions<HealthCheckOptions> healthCheckOptions)
    : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string ApiKeyHeaderName = "api-key";
    private readonly string _apiKey = healthCheckOptions.Value.ApiKey;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? apiKey = Request.Headers[ApiKeyHeaderName];
        if (!_apiKey.Equals(apiKey, StringComparison.InvariantCultureIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Wrong API key."));
        }

        ClaimsPrincipal claimsPrincipal = new();
        ClaimsIdentity claimsIdentity = new("APIKey");
        claimsPrincipal.AddIdentity(claimsIdentity);
        AuthenticationTicket authTicket = new(claimsPrincipal, ApiKeyAuthenticationSchemeOptions.Name);
        return Task.FromResult(AuthenticateResult.Success(authTicket));
    }
}
