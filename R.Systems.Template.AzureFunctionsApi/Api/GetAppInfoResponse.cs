namespace R.Systems.Template.AzureFunctionsApi.Api;

public record GetAppInfoResponse
{
    public string AppName { get; init; } = "";

    public string AppVersion { get; init; } = "";
}
