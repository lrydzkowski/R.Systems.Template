namespace R.Systems.Template.Infrastructure.Azure.Options;

internal class AzureAdB2COptions
{
    public const string Position = "AzureAdB2C";
    public string Instance { get; init; } = "";
    public string ClientId { get; init; } = "";
    public string Domain { get; init; } = "";
    public string SignUpSignInPolicyId { get; init; } = "";
}
