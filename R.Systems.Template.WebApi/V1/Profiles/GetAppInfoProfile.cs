﻿using AutoMapper;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.WebApi.V1.Responses;

namespace R.Systems.Template.WebApi.V1.Profiles;

public class GetAppInfoProfile : Profile
{
    public GetAppInfoProfile()
    {
        CreateMap<GetAppInfoResult, GetAppInfoResponse>();
    }
}
