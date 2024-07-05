using MongoDB.Driver;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Extensions;

internal static class DataExtensions
{
    private const string CountFacetName = "count";
    private const string DataFacetName = "data";

    public static async Task<ListInfo<TModel>> GetDataAsync<TDocument, TModel>(
        this IMongoCollection<TDocument> collection,
        ProjectionDefinition<TDocument, TModel> projection,
        ListParameters listParameters,
        FilterDefinition<TModel>? initialFilter = null,
        CancellationToken cancellationToken = default
    ) where TDocument : class, new() where TModel : class, new()
    {
        FilterDefinition<TModel> filterDefinition = listParameters.Filters.BuildFilterDefinition<TModel>(
            listParameters.Fields
        );
        if (initialFilter != null)
        {
            filterDefinition = Builders<TModel>.Filter.And(initialFilter, filterDefinition);
        }

        SortDefinition<TModel> sortDefinition = listParameters.Sorting.BuildSortDefinition<TModel>(
            listParameters.Fields
        );

        AggregateFacet<TModel, AggregateCountResult> countFacet = AggregateFacet.Create(
            CountFacetName,
            PipelineDefinition<TModel, AggregateCountResult>.Create(
                new[]
                {
                    PipelineStageDefinitionBuilder.Count<TModel>()
                }
            )
        );
        AggregateFacet<TModel, TModel> dataFacet = AggregateFacet.Create(
            DataFacetName,
            PipelineDefinition<TModel, TModel>.Create(
                new[]
                {
                    PipelineStageDefinitionBuilder.Skip<TModel>(
                        (listParameters.Pagination.Page - 1) * listParameters.Pagination.PageSize
                    ),
                    PipelineStageDefinitionBuilder.Limit<TModel>(listParameters.Pagination.PageSize)
                }
            )
        );
        List<AggregateFacetResults> aggregation = await collection.Aggregate()
            .Project(projection)
            .Match(filterDefinition)
            .Sort(sortDefinition)
            .Facet(countFacet, dataFacet)
            .ToListAsync(cancellationToken);

        long count = aggregation.First()
                         .Facets.First(x => x.Name == CountFacetName)
                         .Output<AggregateCountResult>()
                         ?.FirstOrDefault()
                         ?.Count
                     ?? 0;

        IReadOnlyList<TModel>? data = aggregation.First()
            .Facets.First(x => x.Name == DataFacetName)
            .Output<TModel>();

        ListInfo<TModel> listInfo = new()
        {
            Count = count,
            Data = data.ToList()
        };

        return listInfo;
    }
}
