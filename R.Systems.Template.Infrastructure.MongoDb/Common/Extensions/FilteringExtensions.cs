using MongoDB.Driver;
using R.Systems.Template.Core.Common.Extensions;
using R.Systems.Template.Core.Common.Lists;
using System.Reflection;
using System.Text.RegularExpressions;
using FieldInfo = R.Systems.Template.Core.Common.Lists.FieldInfo;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Extensions;

internal static class FilteringExtensions
{
    public static FilterDefinition<TModel> BuildFilterDefinition<TModel>(
        this IReadOnlyList<SearchFilterGroup> searchFilterGroups,
        IReadOnlyList<FieldInfo> fields
    )
    {
        FilterDefinitionBuilder<TModel> builder = Builders<TModel>.Filter;
        searchFilterGroups = RemoveEmptyFilters(searchFilterGroups, fields);
        if (searchFilterGroups.Count == 0)
        {
            return builder.Empty;
        }

        PropertyInfo[] properties = GetEntityProperties<TModel>();
        List<FilterDefinition<TModel>> groups = [];
        foreach (SearchFilterGroup searchFilterGroup in searchFilterGroups)
        {
            List<FilterDefinition<TModel>> subgroups = [];
            foreach (SearchFilter searchFilter in searchFilterGroup.Filters)
            {
                PropertyInfo? property = properties.FirstOrDefault(
                    property => property.Name.CompareIgnoreCase(searchFilter.FieldName)
                );
                if (property == null)
                {
                    continue;
                }

                if (property.PropertyType == typeof(string))
                {
                    string regexPattern = $".*{searchFilter.Value}.*";
                    subgroups.Add(
                        builder.Regex(
                            searchFilter.FieldName!,
                            new Regex(regexPattern, RegexOptions.IgnoreCase)
                        )
                    );
                }
            }

            groups.Add(builder.Or(subgroups));
        }

        FilterDefinition<TModel> filter = builder.And(groups);

        return filter;
    }

    private static List<SearchFilterGroup> RemoveEmptyFilters(
        IReadOnlyList<SearchFilterGroup> searchFilterGroups,
        IReadOnlyList<FieldInfo> fields
    )
    {
        return searchFilterGroups.Where(
                x => x.Filters.Any(
                    y => fields.Any(z => z.FieldName.CompareIgnoreCase(y.FieldName) && z.UseInFiltering)
                )
            )
            .ToList();
    }

    private static PropertyInfo[] GetEntityProperties<T>()
    {
        Type entityType = typeof(T);

        return entityType.GetProperties();
    }
}
