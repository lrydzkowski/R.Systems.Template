using AutoMapper;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

internal class UpdateCompanyCommandProfile : Profile
{
    public UpdateCompanyCommandProfile()
    {
        CreateMap<UpdateCompanyCommand, CompanyToUpdate>();
    }
}
