namespace R.Systems.Template.Infrastructure.Azure.Options;

internal class AzureAdOptions
{
    public const string Position = "AzureAd";

    public string Instance { get; init; } = "";

    public string ClientId { get; init; } = "";

    public string TenantId { get; init; } = "";

    public string Audience { get; init; } = "";
}
