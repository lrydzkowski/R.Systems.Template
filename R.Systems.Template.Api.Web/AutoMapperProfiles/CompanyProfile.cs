﻿using AutoMapper;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;

namespace R.Systems.Template.Api.Web.AutoMapperProfiles;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CreateCompanyRequest, CreateCompanyCommand>();
        CreateMap<UpdateCompanyRequest, UpdateCompanyCommand>()
            .ForMember(dest => dest.CompanyId, opts => opts.Ignore());
    }
}
