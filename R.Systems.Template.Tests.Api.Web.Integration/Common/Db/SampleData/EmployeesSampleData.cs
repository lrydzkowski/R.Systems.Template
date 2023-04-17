using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

internal static class EmployeesSampleData
{
    public static List<EmployeeEntity> Data { get; } = new()
    {
        new EmployeeEntity
        {
            Id = IdGenerator.GetEmployeeId(1),
            FirstName = "John",
            LastName = "Doe",
            CompanyId = IdGenerator.GetCompanyId(1)
        },
        new EmployeeEntity
        {
            Id = IdGenerator.GetEmployeeId(2),
            FirstName = "Jack",
            LastName = "Parker",
            CompanyId = IdGenerator.GetCompanyId(2)
        },
        new EmployeeEntity
        {
            Id = IdGenerator.GetEmployeeId(3),
            FirstName = "Will",
            LastName = "Smith",
            CompanyId = IdGenerator.GetCompanyId(2)
        },
        new EmployeeEntity
        {
            Id = IdGenerator.GetEmployeeId(4),
            FirstName = "James",
            LastName = "Smith",
            CompanyId = IdGenerator.GetCompanyId(2)
        },
        new EmployeeEntity
        {
            Id = IdGenerator.GetEmployeeId(5),
            FirstName = "Maria",
            LastName = "Garcia",
            CompanyId = IdGenerator.GetCompanyId(2)
        },
        new EmployeeEntity
        {
            Id = IdGenerator.GetEmployeeId(6),
            FirstName = "Mary",
            LastName = "Smith",
            CompanyId = IdGenerator.GetCompanyId(2)
        },
        new EmployeeEntity
        {
            Id = IdGenerator.GetEmployeeId(7),
            FirstName = "Maria",
            LastName = "Hernandez",
            CompanyId = IdGenerator.GetCompanyId(2)
        },
        new EmployeeEntity
        {
            Id = IdGenerator.GetEmployeeId(8),
            FirstName = "James",
            LastName = "Johnson",
            CompanyId = IdGenerator.GetCompanyId(2)
        },
        new EmployeeEntity
        {
            Id = IdGenerator.GetEmployeeId(9),
            FirstName = "Kate",
            LastName = "Brown",
            CompanyId = IdGenerator.GetCompanyId(2)
        }
    };

    public static EmployeeEntity Clone(this EmployeeEntity employeeEntity)
    {
        return new EmployeeEntity
        {
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
