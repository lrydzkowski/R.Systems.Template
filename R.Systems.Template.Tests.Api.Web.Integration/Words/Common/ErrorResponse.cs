namespace R.Systems.Template.Tests.Api.Web.Integration.Words.Common;

internal class ErrorResponse
{
    public int? StatusCode { get; init; }

    public string? Error { get; init; }

    public string? Message { get; init; }
}
