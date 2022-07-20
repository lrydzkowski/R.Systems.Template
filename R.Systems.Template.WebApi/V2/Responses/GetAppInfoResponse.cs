namespace R.Systems.Template.WebApi.V2.Responses;

public record GetAppInfoResponse
{
    public string AppName { get; init; } = "";

    public string AppVersion { get; init; } = "";

    public string ApiVersion { get; set; } = "";
}
