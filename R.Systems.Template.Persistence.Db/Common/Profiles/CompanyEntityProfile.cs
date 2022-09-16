using AutoMapper;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Common.Profiles;

internal class CompanyEntityProfile : Profile
{
    public CompanyEntityProfile()
    {
        CreateMap<CompanyEntity, Company>()
            .ForMember(company => company.CompanyId, options => options.MapFrom(companyEntity => companyEntity.Id));
        CreateMap<CompanyToCreate, CompanyEntity>()
            .ForMember(companyEntity => companyEntity.Id, options => options.Ignore())
            .ForMember(companyEntity => companyEntity.Employees, options => options.Ignore());
        CreateMap<CompanyToUpdate, CompanyEntity>()
            .ForMember(companyEntity => companyEntity.Id, options => options.MapFrom(company => company.CompanyId))
            .ForMember(companyEntity => companyEntity.Employees, options => options.Ignore());
    }
}
