namespace R.Systems.Template.Infrastructure.Wordnik.Common.Options;

internal class WordnikOptions
{
    public const string Position = "Wordnik";

    public string ApiBaseUrl { get; init; } = "";

    public string DefinitionsUrl { get; init; } = "";

    public string ApiKey { get; init; } = "";
}
