using MongoDB.Driver;
using R.Systems.Template.Core.Common.Extensions;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Extensions;

public static class SortingExtensions
{
    public static SortDefinition<TModel> BuildSortDefinition<TModel>(
        this Sorting sorting,
        IReadOnlyList<FieldInfo> fields
    )
    {
        IReadOnlyList<Sorting> sortingStages = PrepareSorting(sorting, fields);
        SortDefinitionBuilder<TModel> builder = Builders<TModel>.Sort;
        List<SortDefinition<TModel>> sortDefinitions = [];
        foreach (Sorting sortingStage in sortingStages)
        {
            string fieldName = sortingStage.FieldName!.FirstLetterUpperCase();
            sortDefinitions.Add(
                sortingStage.Order == SortingOrder.Ascending
                    ? builder.Ascending(fieldName)
                    : builder.Descending(fieldName)
            );
        }

        SortDefinition<TModel> definition = builder.Combine(sortDefinitions);

        return definition;
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
}
