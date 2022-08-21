using AutoMapper;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.WebApi.Api;

namespace R.Systems.Template.WebApi.Profiles;

public class GetAppInfoProfile : Profile
{
    public GetAppInfoProfile()
    {
        CreateMap<GetAppInfoResult, GetAppInfoResponse>();
    }
}
