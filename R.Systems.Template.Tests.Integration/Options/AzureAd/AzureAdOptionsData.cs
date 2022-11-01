using R.Systems.Template.Infrastructure.Azure.Options;
using R.Systems.Template.Tests.Integration.Common.Options;

namespace R.Systems.Template.Tests.Integration.Options.AzureAd;

internal class AzureAdOptionsData : IOptionsData
{
    public string? Instance { get; init; }

    public string? ClientId { get; init; }

    public string? TenantId { get; init; }

    public string? Audience { get; init; }

    public AzureAdOptionsData()
    {
        Instance = "https://login.microsoftonline.com/";
        ClientId = "D6F94D05-BA99-4752-824E-B068F8DE9A15";
        TenantId = "5B684592-06A9-42C4-A65C-2AA19DE3F3B3";
        Audience = "https://lrspaceb2c.onmicrosoft.com/D6F94D05-BA99-4752-824E-B068F8DE9A15";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new()
        {
            [$"{AzureAdOptions.Position}:{nameof(AzureAdOptions.Instance)}"] = Instance,
            [$"{AzureAdOptions.Position}:{nameof(AzureAdOptions.ClientId)}"] = ClientId,
            [$"{AzureAdOptions.Position}:{nameof(AzureAdOptions.TenantId)}"] = TenantId,
            [$"{AzureAdOptions.Position}:{nameof(AzureAdOptions.Audience)}"] = Audience
        };
    }
}
