using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Mappers;

[Mapper]
internal partial class CompanyEntityMapper
{
    [MapProperty(nameof(CompanyEntity.Id), nameof(Company.CompanyId))]
    [MapperIgnoreSource(nameof(CompanyEntity.Employees))]
    public partial Company ToCompany(CompanyEntity entity);

    [MapperIgnoreTarget(nameof(CompanyEntity.Id))]
    [MapperIgnoreTarget(nameof(CompanyEntity.Employees))]
    public partial CompanyEntity ToCompanyEntity(CompanyToCreate companyToCreate);

    [MapperIgnoreTarget(nameof(CompanyEntity.Id))]
    [MapperIgnoreTarget(nameof(CompanyEntity.Employees))]
    [MapperIgnoreSource(nameof(CompanyToUpdate.CompanyId))]
    public partial CompanyEntity ToCompanyEntity(CompanyToUpdate companyToUpdate);
}
