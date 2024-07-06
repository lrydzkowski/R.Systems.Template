using System.Linq.Dynamic.Core;
using System.Reflection;
using MassTransit.Internals;
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

                if (property.PropertyType == typeof(string))
                {
                    HandleStringProperty(searchFilter, subGroups, valuesToSubstitute);

                    continue;
                }

                if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                {
                    HandleIntProperty(searchFilter, subGroups, valuesToSubstitute);

                    continue;
                }

                if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
                {
                    HandleLongProperty(searchFilter, subGroups, valuesToSubstitute);

                    continue;
                }


                if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    HandleDecimalProperty(searchFilter, subGroups, valuesToSubstitute);

                    continue;
                }

                if (property.PropertyType == typeof(DateOnly) || property.PropertyType == typeof(DateOnly?))
                {
                    HandleDateProperty(searchFilter, subGroups, valuesToSubstitute);

                    continue;
                }

                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    HandleDateTimeProperty(searchFilter, subGroups, valuesToSubstitute);
                }
            }

            string whereGroupQuery = BuildWhereQuery(searchFilterGroup.Operator, subGroups);
            groups.Add(whereGroupQuery);
        }

        string whereFullQuery = BuildWhereQuery(FilterGroupOperator.And, groups);
        query = query.Where(whereFullQuery, valuesToSubstitute.ToArray());

        return query;
    }

    private static void HandleStringProperty(
        SearchFilter searchFilter,
        List<string> subGroups,
        List<object> valuesToSubstitute
    )
    {
        int index = valuesToSubstitute.Count;
        subGroups.Add(BuildInvariantContainsQuery(searchFilter.FieldName!, index));
        valuesToSubstitute.Add(searchFilter.Value.ToLower());
    }

    private static void HandleIntProperty(
        SearchFilter searchFilter,
        List<string> subGroups,
        List<object> valuesToSubstitute
    )
    {
        int index = valuesToSubstitute.Count;
        subGroups.Add(BuildContainsQuery(searchFilter.FieldName!, index));
        valuesToSubstitute.Add(searchFilter.Value.ToLower());
    }

    private static void HandleLongProperty(
        SearchFilter searchFilter,
        List<string> subGroups,
        List<object> valuesToSubstitute
    )
    {
        int index = valuesToSubstitute.Count;
        subGroups.Add(BuildContainsQuery(searchFilter.FieldName!, index));
        valuesToSubstitute.Add(searchFilter.Value.ToLower());
    }

    private static void HandleDecimalProperty(
        SearchFilter searchFilter,
        List<string> subGroups,
        List<object> valuesToSubstitute
    )
    {
        int index = valuesToSubstitute.Count;
        subGroups.Add(BuildContainsQuery(searchFilter.FieldName!, index));
        valuesToSubstitute.Add(searchFilter.Value.ToLower());
    }

    private static void HandleDateProperty(
        SearchFilter searchFilter,
        List<string> subGroups,
        List<object> valuesToSubstitute
    )
    {
        bool result = DateOnly.TryParseExact(searchFilter.Value, "yyyy-MM-dd", out DateOnly parsed);
        if (!result)
        {
            return;
        }

        int index = valuesToSubstitute.Count;
        subGroups.Add(BuildEqualsQuery(searchFilter.FieldName!, index));
        valuesToSubstitute.Add(parsed);
    }

    private static void HandleDateTimeProperty(
        SearchFilter searchFilter,
        List<string> subGroups,
        List<object> valuesToSubstitute
    )
    {
        int index = valuesToSubstitute.Count;
        subGroups.Add(BuildContainsQuery(searchFilter.FieldName!, index));
        valuesToSubstitute.Add(searchFilter.Value.ToLower());
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
        return $"({fieldName} != null AND {fieldName}.ToString().Contains(@{index}))";
    }

    private static string BuildInvariantContainsQuery(string fieldName, int index)
    {
        return $"({fieldName} != null AND {fieldName}.ToLower().Contains(@{index}))";
    }

    private static string BuildEqualsQuery(string fieldName, int index)
    {
        return $"({fieldName} != null AND {fieldName} = @{index})";
    }

    private static string BuildRangeQuery(string fieldName, int leftIndex, int rightIndex)
    {
        return $"({fieldName} != null AND {fieldName} > @{leftIndex} AND {fieldName} < @{rightIndex})";
    }

    private static string BuildWhereQuery(FilterGroupOperator filterGroupOperator, List<string> parts)
    {
        string logicOperator = GetLogicOperator(filterGroupOperator);
        string whereQuery = "(" + string.Join($") {logicOperator} (", parts) + ")";

        return whereQuery;
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
