using AutoMapper;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Api.AzureFunctions.AutoMapperProfiles;

public class ListsProfile : Profile
{
    public ListsProfile()
    {
        CreateMap<ListRequest, ListParameters>()
            .ForMember(
                parameters => parameters.Pagination,
                options => options.MapFrom(
                    request => new Pagination
                    {
                        Page = request.Page,
                        PageSize = request.PageSize
                    }
                )
            )
            .ForMember(
                parameters => parameters.Search,
                options => options.MapFrom(
                    request => new Search
                    {
                        Query = request.SearchQuery
                    }
                )
            )
            .ForMember(
                parameters => parameters.Sorting,
                options => options.MapFrom(
                    request => new Sorting
                    {
                        FieldName = request.SortingFieldName,
                        Order = MapToSortingOrder(request.SortingOrder)
                    }
                )
            );
    }

    private SortingOrder MapToSortingOrder(string sortingOrder)
    {
        return sortingOrder switch
        {
            "desc" => SortingOrder.Descending,
            _ => SortingOrder.Ascending
        };
    }
}
