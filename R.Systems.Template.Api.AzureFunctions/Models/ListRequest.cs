namespace R.Systems.Template.Api.AzureFunctions.Models;

public class ListRequest
{
    public int Page { get; init; }
    public int PageSize { get; init; } = 100;
    public string? SortingFieldName { get; init; }
    public string SortingOrder { get; init; } = "";
    public string? SearchQuery { get; init; }
}
