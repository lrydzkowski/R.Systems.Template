using R.Systems.Template.Tests.Integration.Common.Options;

namespace R.Systems.Template.Tests.Integration.Options.AzureAd;

internal class AzureAdOptionsData : IOptionsData
{
    public string? Instance { get; init; }

    public string? Domain { get; init; }

    public string? ClientId { get; init; }

    public string? SignUpSignInPolicyId { get; init; }

    public AzureAdOptionsData()
    {
        Instance = "https://rsystems.b2clogin.com";
        Domain = "rsystems.onmicrosoft.com";
        ClientId = "D6F94D05-BA99-4752-824E-B068F8DE9A15";
        SignUpSignInPolicyId = "B2C_1A_SIGNUP_SIGNIN";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new()
        {
            ["AzureAd:Instance"] = Instance,
            ["AzureAd:Domain"] = Domain,
            ["AzureAd:ClientId"] = ClientId,
            ["AzureAd:SignUpSignInPolicyId"] = SignUpSignInPolicyId
        };
    }
}
