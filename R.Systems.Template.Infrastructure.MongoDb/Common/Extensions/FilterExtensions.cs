using MongoDB.Driver;
using R.Systems.Template.Core.Common.Extensions;
using R.Systems.Template.Core.Common.Lists;
using System.Reflection;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Extensions;

internal static class FilterExtensions
{
    public static FilterDefinition<TDocument> BuilderFilterDefinition<TDocument>(
        Search search,
        IReadOnlyList<string> fieldsAvailableToFilter
    )
    {
        FilterDefinitionBuilder<TDocument> builder = Builders<TDocument>.Filter;
        if (search.Query == null)
        {
            return builder.Empty;
        }

        List<FilterDefinition<TDocument>> filters = [];
        PropertyInfo[] properties = GetEntityProperties<TDocument>();
        foreach (string fieldName in fieldsAvailableToFilter)
        {
            PropertyInfo? property = properties.FirstOrDefault(
                property => property.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase)
            );
            if (property == null)
            {
                continue;
            }

            filters.Add(builder.Regex(fieldName.FirstLetterUpperCase(), $"/.*{search.Query}.*/"));
        }

        FilterDefinition<TDocument> filter = builder.Or(filters);

        return filter;
    }

    private static PropertyInfo[] GetEntityProperties<T>()
    {
        Type entityType = typeof(T);

        return entityType.GetProperties();
    }
}
