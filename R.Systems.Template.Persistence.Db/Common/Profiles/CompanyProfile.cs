using AutoMapper;
using R.Systems.Template.Core.Common.DataTransferObjects;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Common.Profiles;

internal class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CompanyEntity, CompanyDto>();
    }
}
