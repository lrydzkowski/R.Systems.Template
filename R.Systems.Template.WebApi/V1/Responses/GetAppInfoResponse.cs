namespace R.Systems.Template.WebApi.V1.Responses;

public record GetAppInfoResponse
{
    public string AppName { get; init; } = "";

    public string AppVersion { get; init; } = "";
}
