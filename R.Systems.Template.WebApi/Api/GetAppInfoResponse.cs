namespace R.Systems.Template.WebApi.Api;

public record GetAppInfoResponse
{
    public string AppName { get; init; } = "";

    public string AppVersion { get; init; } = "";
}
