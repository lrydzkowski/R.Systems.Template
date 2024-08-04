using Azure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace R.Systems.Template.Infrastructure.Azure.Authentication;

public interface IAccessTokenProvider
{
    Task<string?> ProvideAsync(string serviceClientId, CancellationToken cancellationToken);
}

public class AccessTokenProvider : IAccessTokenProvider
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccessTokenProvider> _logger;

    public AccessTokenProvider(IConfiguration configuration, ILogger<AccessTokenProvider> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string?> ProvideAsync(string serviceClientId, CancellationToken cancellationToken)
    {
        TokenCredential? credential = TokenCredentialProvider.Provide(_configuration);
        if (credential == null)
        {
            _logger.LogError("TokenCredential is not available for generating an access token.");

            return null;
        }

        AccessToken? accessToken = await credential.GetTokenAsync(
            new TokenRequestContext([BuildScope(serviceClientId)]),
            cancellationToken
        );

        return accessToken?.Token;
    }

    private string BuildScope(string serviceClientId)
    {
        return $"{serviceClientId}/.default";
    }
}
