namespace R.Systems.Template.Api.AzureFunctions.Models;

public record GetAppInfoResponse
{
    public string AppName { get; init; } = "";
    public string AppVersion { get; init; } = "";
}
