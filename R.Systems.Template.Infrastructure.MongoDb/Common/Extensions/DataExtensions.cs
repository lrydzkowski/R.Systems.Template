﻿using MongoDB.Driver;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Extensions;

internal static class DataExtensions
{
    private const string CountFacetName = "count";
    private const string DataFacetName = "data";

    public static async Task<ListInfo<TDocument>> GetDataAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        ListParameters listParameters,
        IReadOnlyList<string> fieldsAvailableToFilter,
        IReadOnlyList<string> fieldsAvailableToSort,
        string defaultSortingFieldName,
        FilterDefinition<TDocument>? initialFilter = null,
        CancellationToken cancellationToken = default
    ) where TDocument : class, new()
    {
        FilterDefinition<TDocument> filterDefinition = listParameters.Search.BuildFilterDefinition<TDocument>(
            fieldsAvailableToFilter
        );
        if (initialFilter != null)
        {
            filterDefinition = Builders<TDocument>.Filter.And(initialFilter, filterDefinition);
        }

        SortDefinition<TDocument> sortDefinition = listParameters.Sorting.BuildSortDefinition<TDocument>(
            fieldsAvailableToSort,
            defaultSortingFieldName
        );

        AggregateFacet<TDocument, AggregateCountResult> countFacet = AggregateFacet.Create(
            CountFacetName,
            PipelineDefinition<TDocument, AggregateCountResult>.Create(
                new[]
                {
                    PipelineStageDefinitionBuilder.Count<TDocument>()
                }
            )
        );
        AggregateFacet<TDocument, TDocument> dataFacet = AggregateFacet.Create(
            DataFacetName,
            PipelineDefinition<TDocument, TDocument>.Create(
                new[]
                {
                    PipelineStageDefinitionBuilder.Skip<TDocument>(
                        (listParameters.Pagination.Page - 1) * listParameters.Pagination.PageSize
                    ),
                    PipelineStageDefinitionBuilder.Limit<TDocument>(listParameters.Pagination.PageSize)
                }
            )
        );
        List<AggregateFacetResults> aggregation = await collection.Aggregate()
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

        IReadOnlyList<TDocument>? data = aggregation.First()
            .Facets.First(x => x.Name == DataFacetName)
            .Output<TDocument>();

        ListInfo<TDocument> listInfo = new()
        {
            Count = count,
            Data = data.ToList()
        };

        return listInfo;
    }
}
