using AutoMapper;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

internal class CreateCompanyCommandProfile : Profile
{
    public CreateCompanyCommandProfile()
    {
        CreateMap<CreateCompanyCommand, CompanyToCreate>();
    }
}
