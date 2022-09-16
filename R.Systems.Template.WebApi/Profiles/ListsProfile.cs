using AutoMapper;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.WebApi.Api;

namespace R.Systems.Template.WebApi.Profiles;

public class ListsProfile : Profile
{
    public ListsProfile()
    {
        CreateMap<ListRequest, ListParameters>()
            .ForMember(parameters => parameters.Pagination, options => options.MapFrom(request => new Pagination { FirstIndex = request.FirstIndex, NumberOfRows = request.NumberOfRows }))
            .ForMember(parameters => parameters.Search, options => options.MapFrom(request => new Search { Query = request.SearchQuery }))
            .ForMember(parameters => parameters.Sorting, options => options.MapFrom(request => new Sorting { FieldName = request.SortingFieldName, Order = SortingOrder.Ascending }));
    }
}
