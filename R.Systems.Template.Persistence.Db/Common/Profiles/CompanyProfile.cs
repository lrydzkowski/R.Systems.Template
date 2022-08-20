using AutoMapper;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Common.Profiles;

internal class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CompanyEntity, Company>();
    }
}
