using R.Systems.Template.Infrastructure.Azure.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAdB2C;

internal class AzureAdB2COptionsData : AzureAdB2COptions, IOptionsData
{
    public AzureAdB2COptionsData()
    {
        Instance = "https://test.b2clogin.com";
        ClientId = "A387363D-9FAC-4F69-AED5-5462671AB749";
        Domain = "test.onmicrosoft.com";
        SignUpSignInPolicyId = "B2C_1_SIGN_UP_SIGN_IN";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(Instance)}"] = Instance,
            [$"{Position}:{nameof(ClientId)}"] = ClientId,
            [$"{Position}:{nameof(Domain)}"] = Domain,
            [$"{Position}:{nameof(SignUpSignInPolicyId)}"] = SignUpSignInPolicyId
        };
    }
}
