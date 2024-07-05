using System.Linq.Dynamic.Core;
using System.Reflection;
using R.Systems.Template.Core.Common.Extensions;

namespace R.Systems.Template.Core.Common.Lists.Extensions;

public static class FilteringExtensions
{
    public static IQueryable<T> Filter<T>(
        this IQueryable<T> query,
        IReadOnlyList<SearchFilterGroup> searchFilterGroups,
        IReadOnlyList<FieldInfo> fields
    )
    {
        searchFilterGroups = RemoveEmptyFilters(searchFilterGroups, fields);
        if (searchFilterGroups.Count == 0)
        {
            return query;
        }

        PropertyInfo[] properties = GetEntityProperties<T>();
        List<string> groups = [];
        List<object> valuesToSubstitute = [];
        foreach (SearchFilterGroup searchFilterGroup in searchFilterGroups)
        {
            List<string> subGroups = [];
            foreach (SearchFilter searchFilter in searchFilterGroup.Filters)
            {
                PropertyInfo? property = properties.FirstOrDefault(
                    property => property.Name.CompareIgnoreCase(searchFilter.FieldName)
                );
                if (property == null)
                {
                    continue;
                }

                string? whereQuery = null;
                int index = valuesToSubstitute.Count;
                if (property.PropertyType == typeof(string))
                {
                    whereQuery = BuildContainsQuery(searchFilter.FieldName!, index);
                }

                if (whereQuery == null)
                {
                    continue;
                }

                subGroups.Add(whereQuery);
                valuesToSubstitute.Add(searchFilter.Value.ToLower());
            }

            string whereGroupQuery = BuildWherePart(searchFilterGroup.Operator, subGroups);
            groups.Add(whereGroupQuery);
        }

        string whereFullQuery = BuildWherePart(FilterGroupOperator.And, groups);
        query = query.Where(whereFullQuery, valuesToSubstitute.ToArray());

        return query;
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

    private static string BuildContainsQuery(string fieldName, int index)
    {
        return $"{fieldName}.ToLower().Contains(@{index})";
    }

    private static string BuildWherePart(FilterGroupOperator filterGroupOperator, List<string> parts)
    {
        string logicOperator = GetLogicOperator(filterGroupOperator);
        string wherePart = "(" + string.Join($") {logicOperator} (", parts) + ")";

        return wherePart;
    }

    private static string GetLogicOperator(FilterGroupOperator filterGroupOperator)
    {
        return filterGroupOperator switch
        {
            FilterGroupOperator.And => "AND",
            FilterGroupOperator.Or => "OR",
            _ => "OR"
        };
    }
}
