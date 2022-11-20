using System.Linq.Dynamic.Core;

namespace R.Systems.Template.Core.Common.Lists.Extensions;

public static class SortExtensions
{
    public static IQueryable<T> Sort<T>(
        this IQueryable<T> query,
        List<string> fieldsAvailableToSort,
        Sorting sorting,
        string defaultSortingFieldName
    )
    {
        sorting = PrepareSortingParameters(sorting, defaultSortingFieldName);

        if (!fieldsAvailableToSort.Contains(sorting.FieldName!))
        {
            return query;
        }

        string sortOrderQuery = sorting.Order == SortingOrder.Ascending ? "" : " desc";
        string sortQuery = $"{sorting.FieldName}{sortOrderQuery}, {defaultSortingFieldName} asc";
        query = query.OrderBy(sortQuery);

        return query;
    }

    private static Sorting PrepareSortingParameters(Sorting sorting, string defaultSortingFieldName)
    {
        if (sorting.FieldName != null)
        {
            return sorting;
        }

        return new Sorting
        {
            FieldName = defaultSortingFieldName,
            Order = SortingOrder.Ascending
        };
    }
}
