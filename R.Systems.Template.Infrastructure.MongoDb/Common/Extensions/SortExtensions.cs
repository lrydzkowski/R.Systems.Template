using MongoDB.Driver;
using R.Systems.Template.Core.Common.Extensions;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Extensions;

public static class SortExtensions
{
    public static SortDefinition<TDocument> BuildSortDefinition<TDocument>(
        Sorting sorting,
        IReadOnlyList<string> fieldsAvailableToSort,
        string defaultSortingFieldName
    )
    {
        sorting = PrepareSortingParameters(sorting, fieldsAvailableToSort, defaultSortingFieldName);

        string fieldName = sorting.FieldName!.FirstLetterUpperCase();
        SortDefinitionBuilder<TDocument> builder = Builders<TDocument>.Sort;
        SortDefinition<TDocument> definition = sorting.Order == SortingOrder.Ascending
            ? builder.Ascending(fieldName)
            : builder.Descending(fieldName);

        return definition;
    }

    private static Sorting PrepareSortingParameters(
        Sorting sorting,
        IReadOnlyList<string> fieldsAvailableToSort,
        string defaultSortingFieldName
    )
    {
        if (sorting.FieldName != null
            && fieldsAvailableToSort.Contains(sorting.FieldName, StringComparer.CurrentCultureIgnoreCase))
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
