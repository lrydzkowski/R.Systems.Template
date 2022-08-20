using AutoMapper;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

internal class CreateCompanyCommandProfile : Profile
{
    public CreateCompanyCommandProfile()
    {
        CreateMap<CreateCompanyCommand, CompanyToCreate>();
    }
}
