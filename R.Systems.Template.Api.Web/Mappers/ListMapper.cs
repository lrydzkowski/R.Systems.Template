using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Lists;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Api.Web.Mappers;

[Mapper]
public partial class ListMapper
{
    [MapProperty(
        new[] { nameof(ListRequest.Page) },
        new[] { nameof(ListParameters.Pagination), nameof(ListParameters.Pagination.Page) }
    )]
    [MapProperty(
        new[] { nameof(ListRequest.PageSize) },
        new[] { nameof(ListParameters.Pagination), nameof(ListParameters.Pagination.PageSize) }
    )]
    [MapProperty(
        new[] { nameof(ListRequest.SortingFieldName) },
        new[] { nameof(ListParameters.Sorting), nameof(ListParameters.Sorting.FieldName) }
    )]
    [MapProperty(
        new[] { nameof(ListRequest.SortingOrder) },
        new[] { nameof(ListParameters.Sorting), nameof(ListParameters.Sorting.Order) }
    )]
    [MapProperty(
        new[] { nameof(ListRequest.SearchQuery) },
        new[] { nameof(ListParameters.Search), nameof(ListParameters.Search.Query) }
    )]
    public partial ListParameters ToListParameter(ListRequest request);

    private SortingOrder MapToSortingOrder(string sortingOrder)
    {
        return sortingOrder switch
        {
            "desc" => SortingOrder.Descending,
            _ => SortingOrder.Ascending
        };
    }
}
