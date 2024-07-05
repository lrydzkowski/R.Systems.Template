namespace R.Systems.Template.Core.Common.Lists;

public class ListParametersDto
{
    public List<string> FieldsToReturn { get; init; } = [];

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 100;

    public string? SortingFieldName { get; init; }

    public string SortingOrder { get; init; } = "asc";

    public string? SearchQuery { get; init; }
}
