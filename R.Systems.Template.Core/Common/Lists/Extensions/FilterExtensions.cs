using System.Linq.Dynamic.Core;
using System.Reflection;

namespace R.Systems.Template.Core.Common.Lists.Extensions;

public static class FilterExtensions
{
    private static ParsingConfig DynamicLinqParsingConfig { get; } = new()
    {
        ResolveTypesBySimpleName = true
    };

    public static IQueryable<T> Filter<T>(this IQueryable<T> query, List<string> fieldsAvailableToFilter, Search search)
    {
        if (search.Query == null)
        {
            return query;
        }

        PropertyInfo[] properties = GetEntityProperties<T>();
        List<string> whereQueryParts = new();
        foreach (string fieldName in fieldsAvailableToFilter)
        {
            PropertyInfo? property = properties.FirstOrDefault(
                property => property.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase)
            );
            if (property == null)
            {
                continue;
            }

            string? whereQueryPart = null;
            int index = whereQueryParts.Count;
            if (property.PropertyType == typeof(string))
            {
                whereQueryPart = GetStringWhereQuery(fieldName, index);
            }

            if (whereQueryPart == null)
            {
                continue;
            }

            whereQueryParts.Add(whereQueryPart);
        }

        if (whereQueryParts.Count == 0)
        {
            return query;
        }

        string value = search.Query.ToLower();
        string whereQuery = "(" + string.Join(") OR (", whereQueryParts) + ")";
        List<object> valuesToSubstitute =
            Enumerable.Range(1, whereQueryParts.Count).Select(_ => value).ToList<object>();
        query = query.Where(DynamicLinqParsingConfig, whereQuery, valuesToSubstitute.ToArray());

        return query;
    }

    private static PropertyInfo[] GetEntityProperties<T>()
    {
        Type entityType = typeof(T);

        return entityType.GetProperties();
    }

    private static string GetStringWhereQuery(string fieldName, int index)
    {
        return $"{fieldName}.ToLower().Contains(@{index})";
    }
}
