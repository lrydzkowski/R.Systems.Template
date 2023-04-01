namespace R.Systems.Template.Core.Common.Lists;

public class ListParameters
{
    public Pagination Pagination { get; set; } = new();

    public Sorting Sorting { get; set; } = new();

    public Search Search { get; set; } = new();
}

public class Pagination
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 100;
}

public class Sorting
{
    public string? FieldName { get; set; }

    public SortingOrder Order { get; set; } = SortingOrder.Ascending;
}

public enum SortingOrder
{
    Ascending,
    Descending
}

public class Search
{
    public string? Query { get; set; }
}
