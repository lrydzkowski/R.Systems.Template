using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Api.AzureFunctions.Mappers;

[Mapper]
internal partial class CompanyMapper
{
    public partial CreateCompanyCommand ToCreateCommand(CreateCompanyRequest? request);

    [MapperIgnoreTarget(nameof(UpdateCompanyCommand.CompanyId))]
    public partial UpdateCompanyCommand ToUpdateCommand(UpdateCompanyRequest? request);
}
