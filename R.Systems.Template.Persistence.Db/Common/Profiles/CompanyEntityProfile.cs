using AutoMapper;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Common.Profiles;

internal class CompanyEntityProfile : Profile
{
    public CompanyEntityProfile()
    {
        CreateMap<CompanyEntity, Company>()
            .ForMember(company => company.CompanyId, options => options.MapFrom(companyEntity => companyEntity.Id));
        CreateMap<CompanyToCreate, CompanyEntity>();
    }
}
