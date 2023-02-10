using AutoMapper;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Core.App.Queries.GetAppInfo;

namespace R.Systems.Template.Api.AzureFunctions.AutoMapperProfiles;

internal class GetAppInfoProfile : Profile
{
    public GetAppInfoProfile()
    {
        CreateMap<GetAppInfoResult, GetAppInfoResponse>();
    }
}
