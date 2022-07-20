namespace R.Systems.Template.WebApi.Responses;

public record GetAppInfoResponse
{
    public string AppName { get; init; } = "";

    public string AppVersion { get; init; } = "";
}
