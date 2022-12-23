using Microsoft.AspNetCore.Mvc;

namespace R.Systems.Template.Api.Web.Models;

public class ListRequest
{
    [FromQuery(Name = "page")] public int Page { get; init; }

    [FromQuery(Name = "pageSize")] public int PageSize { get; init; } = 100;

    [FromQuery(Name = "sortingFieldName")] public string? SortingFieldName { get; init; }

    [FromQuery(Name = "sortingOrder")] public string SortingOrder { get; init; } = "";

    [FromQuery(Name = "searchQuery")] public string? SearchQuery { get; init; }
}
