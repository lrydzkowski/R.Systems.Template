namespace R.Systems.Template.Infrastructure.CosmosDb.Common.Options;

internal class CosmosDbOptions
{
    public const string Position = "CosmosDb";

    public string AccountUri { get; init; } = "";

    public string DatabaseName { get; init; } = "";
}
