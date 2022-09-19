using Microsoft.AspNetCore.Mvc;

namespace R.Systems.Template.WebApi.Api;

public class ListRequest
{
    [FromQuery(Name = "firstIndex")]
    public int FirstIndex { get; init; }

    [FromQuery(Name = "numberOfRows")]
    public int NumberOfRows { get; init; } = 100;

    [FromQuery(Name = "sortingFieldName")]
    public string? SortingFieldName { get; init; }

    [FromQuery(Name = "sortingOrder")]
    public string SortingOrder { get; init; } = "";

    [FromQuery(Name = "searchQuery")]
    public string? SearchQuery { get; init; }
}
