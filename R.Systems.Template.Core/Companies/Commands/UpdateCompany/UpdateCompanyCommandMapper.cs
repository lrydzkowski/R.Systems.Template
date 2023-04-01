using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

[Mapper]
internal partial class UpdateCompanyCommandMapper
{
    public partial CompanyToUpdate ToCompanyToUpdate(UpdateCompanyCommand command);
}
