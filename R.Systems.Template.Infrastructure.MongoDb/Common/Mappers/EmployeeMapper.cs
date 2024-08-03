using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Mappers;

internal interface IEmployeeMapper
{
    EmployeeDocument Map(EmployeeToCreate employeeToCreate);
    EmployeeDocument Map(EmployeeToUpdate employeeToUpdate);
    List<Employee> Map(List<EmployeeDocument> employeeDocuments);
    Employee Map(EmployeeDocument employeeDocument);
}

internal class EmployeeMapper
    : IEmployeeMapper
{
    public EmployeeDocument Map(EmployeeToCreate employeeToCreate)
    {
        EmployeeDocument employeeDocument = new()
        {
            Id = Ulid.NewUlid().ToGuid(),
            FirstName = employeeToCreate.FirstName,
            LastName = employeeToCreate.LastName,
            CompanyId = employeeToCreate.CompanyId
        };

        return employeeDocument;
    }

    public EmployeeDocument Map(EmployeeToUpdate employeeToUpdate)
    {
        EmployeeDocument employeeDocument = new()
        {
            Id = employeeToUpdate.EmployeeId,
            FirstName = employeeToUpdate.FirstName,
            LastName = employeeToUpdate.LastName,
            CompanyId = employeeToUpdate.CompanyId
        };

        return employeeDocument;
    }

    public List<Employee> Map(List<EmployeeDocument> employeeDocuments)
    {
        return employeeDocuments.Select(Map).ToList();
    }

    public Employee Map(EmployeeDocument employeeDocument)
    {
        Employee employee = new()
        {
            EmployeeId = employeeDocument.Id,
            FirstName = employeeDocument.FirstName,
            LastName = employeeDocument.LastName,
            CompanyId = employeeDocument.CompanyId
        };

        return employee;
    }
}
