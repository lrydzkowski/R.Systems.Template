using R.Systems.Template.Infrastructure.Azure.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAd;

internal class AzureAdOptionsData : AzureAdOptions, IOptionsData
{
    public AzureAdOptionsData()
    {
        Instance = "https://login.microsoftonline.com/";
        ClientId = "D6F94D05-BA99-4752-824E-B068F8DE9A15";
        TenantId = "5B684592-06A9-42C4-A65C-2AA19DE3F3B3";
        Audience = "https://lrspaceb2c.onmicrosoft.com/D6F94D05-BA99-4752-824E-B068F8DE9A15";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(Instance)}"] = Instance,
            [$"{Position}:{nameof(ClientId)}"] = ClientId,
            [$"{Position}:{nameof(TenantId)}"] = TenantId,
            [$"{Position}:{nameof(Audience)}"] = Audience
        };
    }
}
