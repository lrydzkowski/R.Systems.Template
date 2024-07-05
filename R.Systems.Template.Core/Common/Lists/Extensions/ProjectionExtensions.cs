using System.Reflection;
using R.Systems.Template.Core.Common.Extensions;

namespace R.Systems.Template.Core.Common.Lists.Extensions;

public static class ProjectionExtensions
{
    public static IQueryable<T> Project<T>(this IQueryable<T> query, IReadOnlyList<FieldInfo> fields)
    {
        query = query.Select(x => Project(x, fields));

        return query;
    }

    private static T Project<T>(T obj, IReadOnlyList<FieldInfo> fields)
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            if (!property.CanWrite)
            {
                continue;
            }

            if (fields.Any(x => x.FieldName.CompareIgnoreCase(property.Name)))
            {
                continue;
            }

            object? defaultValue = property.PropertyType.IsValueType
                ? Activator.CreateInstance(property.PropertyType)
                : null;
            property.SetValue(obj, defaultValue);
        }

        return obj;
    }
}
