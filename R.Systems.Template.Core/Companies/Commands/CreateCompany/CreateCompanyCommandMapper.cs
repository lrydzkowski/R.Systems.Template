using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

[Mapper]
internal partial class CreateCompanyCommandMapper
{
    public partial CompanyToCreate ToCompanyToCreate(CreateCompanyCommand command);
}
