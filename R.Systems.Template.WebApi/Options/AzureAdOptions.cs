namespace R.Systems.Template.WebApi.Options;

internal class AzureAdOptions
{
    public const string Position = "AzureAd";

    public string Instance { get; init; } = "";

    public string Domain { get; init; } = "";

    public string ClientId { get; init; } = "";

    public string SignUpSignInPolicyId { get; init; } = "";
}
