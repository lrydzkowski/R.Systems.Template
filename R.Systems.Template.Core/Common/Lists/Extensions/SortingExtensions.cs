using System.Linq.Dynamic.Core;
using R.Systems.Template.Core.Common.Extensions;

namespace R.Systems.Template.Core.Common.Lists.Extensions;

public static class SortingExtensions
{
    public static IQueryable<T> Sort<T>(
        this IQueryable<T> query,
        Sorting sorting,
        IReadOnlyList<FieldInfo> fields
    )
    {
        IReadOnlyList<Sorting> sortingStages = PrepareSorting(sorting, fields);
        string sortingText = string.Join(", ", sortingStages.Select(GetSortingStageText));
        query = query.OrderBy(sortingText);

        return query;
    }

    private static IReadOnlyList<Sorting> PrepareSorting(
        Sorting sorting,
        IReadOnlyList<FieldInfo> fields
    )
    {
        if (!CanBeUsedInSorting(sorting, fields))
        {
            return [GetDefaultSorting(sorting)];
        }

        if (sorting.FieldName!.CompareIgnoreCase(sorting.DefaultFieldName))
        {
            return [sorting];
        }

        return [sorting, GetDefaultSorting(sorting)];
    }

    private static bool CanBeUsedInSorting(
        Sorting sorting,
        IReadOnlyList<FieldInfo> fields
    )
    {
        FieldInfo? field = fields.FirstOrDefault(
            x => x.FieldName.CompareIgnoreCase(sorting.FieldName ?? "") && x.UseInSorting
        );

        return field != null;
    }

    private static Sorting GetDefaultSorting(Sorting sorting)
    {
        return new Sorting { FieldName = sorting.DefaultFieldName, Order = SortingOrder.Ascending };
    }

    private static string GetSortingStageText(Sorting sorting)
    {
        return $"{sorting.FieldName?.FirstLetterUpperCase()} {MapToSortingDirectionText(sorting.Order)}";
    }

    private static string MapToSortingDirectionText(SortingOrder sortingOrder)
    {
        return sortingOrder switch
        {
            SortingOrder.Descending => "Desc",
            SortingOrder.Ascending => "Asc",
            _ => "Asc"
        };
    }
}
