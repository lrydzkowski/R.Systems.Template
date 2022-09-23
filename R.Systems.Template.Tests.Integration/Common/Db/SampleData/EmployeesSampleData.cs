using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Tests.Integration.Common.Db.SampleData;

internal static class EmployeesSampleData
{
    public static List<EmployeeEntity> Data { get; } = new()
    {
        new EmployeeEntity
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            CompanyId = 1
        },
        new EmployeeEntity
        {
            Id = 2,
            FirstName = "Jack",
            LastName = "Parker",
            CompanyId = 2
        },
        new EmployeeEntity
        {
            Id = 3,
            FirstName = "Will",
            LastName = "Smith",
            CompanyId = 2
        },
        new EmployeeEntity
        {
            Id = 4,
            FirstName = "James",
            LastName = "Smith",
            CompanyId = 2
        },
        new EmployeeEntity
        {
            Id = 5,
            FirstName = "Maria",
            LastName = "Garcia",
            CompanyId = 2
        },
        new EmployeeEntity
        {
            Id = 6,
            FirstName = "Mary",
            LastName = "Smith",
            CompanyId = 2
        },
        new EmployeeEntity
        {
            Id = 7,
            FirstName = "Maria",
            LastName = "Hernandez",
            CompanyId = 2
        },
        new EmployeeEntity
        {
            Id = 8,
            FirstName = "James",
            LastName = "Johnson",
            CompanyId = 2
        }
    };

    public static EmployeeEntity Clone(this EmployeeEntity employeeEntity)
    {
        return new EmployeeEntity
        {
            Id = employeeEntity.Id,
            FirstName = employeeEntity.FirstName,
            LastName = employeeEntity.LastName,
            CompanyId = employeeEntity.CompanyId
        };
    }

    public static List<Employee> Employees
    {
        get
        {
            return Data
                .Select(
                    x => new Employee
                    {
                        EmployeeId = (int)x.Id!,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        CompanyId = x.CompanyId
                    }
                )
                .ToList();
        }
    }
}
