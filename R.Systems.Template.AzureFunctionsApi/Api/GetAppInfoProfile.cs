using AutoMapper;
using R.Systems.Template.Core.App.Queries.GetAppInfo;

namespace R.Systems.Template.AzureFunctionsApi.Api;

internal class GetAppInfoProfile : Profile
{
    public GetAppInfoProfile()
    {
        CreateMap<GetAppInfoResult, GetAppInfoResponse>();
    }
}
