using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Mappers;

[Mapper]
internal partial class EmployeeEntityMapper
{
    [MapProperty(nameof(EmployeeEntity.Id), nameof(Employee.EmployeeId))]
    [MapperIgnoreSource(nameof(EmployeeEntity.Company))]
    public partial Employee ToEmployee(EmployeeEntity entity);

    [MapperIgnoreTarget(nameof(EmployeeEntity.Id))]
    [MapperIgnoreTarget(nameof(EmployeeEntity.Company))]
    public partial EmployeeEntity ToEmployeeEntity(EmployeeToCreate employeeToCreate);
}
