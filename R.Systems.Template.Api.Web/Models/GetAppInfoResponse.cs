namespace R.Systems.Template.Api.Web.Models;

public record GetAppInfoResponse
{
    public string AppName { get; init; } = "";

    public string AppVersion { get; init; } = "";

    public string Test { get; init; } = ":)";

    public string Test2 { get; init; } = ":) 2";
}
