using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Services;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Items;

namespace R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;

internal interface IEmployeeMapper
{
    EmployeeItem Map(EmployeeToCreate employeeToCreate);
    EmployeeItem Map(EmployeeToUpdate employeeToUpdate);
    List<Employee> Map(List<EmployeeItem> employeeItems);
    Employee Map(EmployeeItem employeeItem);
}

internal class EmployeeMapper
    : IEmployeeMapper
{
    private readonly IUniqueIdGenerator _uniqueIdGenerator;

    public EmployeeMapper(IUniqueIdGenerator uniqueIdGenerator)
    {
        _uniqueIdGenerator = uniqueIdGenerator;
    }

    public EmployeeItem Map(EmployeeToCreate employeeToCreate)
    {
        EmployeeItem employeeItem = new()
        {
            Id = _uniqueIdGenerator.Generate(),
            FirstName = employeeToCreate.FirstName,
            LastName = employeeToCreate.LastName,
            CompanyId = employeeToCreate.CompanyId
        };

        return employeeItem;
    }

    public EmployeeItem Map(EmployeeToUpdate employeeToUpdate)
    {
        EmployeeItem employeeItem = new()
        {
            Id = employeeToUpdate.EmployeeId,
            FirstName = employeeToUpdate.FirstName,
            LastName = employeeToUpdate.LastName,
            CompanyId = employeeToUpdate.CompanyId
        };

        return employeeItem;
    }

    public List<Employee> Map(List<EmployeeItem> employeeItems)
    {
        return employeeItems.Select(Map).ToList();
    }

    public Employee Map(EmployeeItem employeeItem)
    {
        Employee employee = new()
        {
            EmployeeId = employeeItem.Id,
            FirstName = employeeItem.FirstName,
            LastName = employeeItem.LastName,
            CompanyId = employeeItem.CompanyId
        };

        return employee;
    }
}
