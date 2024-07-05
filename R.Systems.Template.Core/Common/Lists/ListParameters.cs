namespace R.Systems.Template.Core.Common.Lists;

public class ListParameters
{
    public IReadOnlyList<FieldInfo> Fields { get; init; } = [];
    public Pagination Pagination { get; init; } = new();
    public Sorting Sorting { get; init; } = new();
    public IReadOnlyList<SearchFilterGroup> Filters { get; init; } = [];
}

public class Pagination
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 100;
}

public class Sorting
{
    public string? FieldName { get; init; }
    public string DefaultFieldName { get; init; } = "";
    public SortingOrder Order { get; init; } = SortingOrder.Ascending;
}

public enum SortingOrder
{
    Ascending,
    Descending
}

public class SearchFilterGroup
{
    public FilterGroupOperator Operator { get; init; }
    public IReadOnlyList<SearchFilter> Filters { get; init; } = [];
}

public enum FilterGroupOperator
{
    And,
    Or
}

public class SearchFilter
{
    public string? FieldName { get; init; }
    public string Value { get; init; } = "";
}
