using AutoMapper;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.WebApi.V2.Responses;

namespace R.Systems.Template.WebApi.V2.Profiles;

public class GetAppInfoProfile : Profile
{
    public GetAppInfoProfile()
    {
        CreateMap<GetAppInfoResult, GetAppInfoResponse>()
            .ForMember(
                response => response.ApiVersion,
                o => o.MapFrom(_ => "2.0")
            );
    }
}
