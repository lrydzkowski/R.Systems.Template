using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Employees;

public class GetEmployeesResponseExamples : IMultipleExamplesProvider<ListInfo<Employee>>
{
    public IEnumerable<SwaggerExample<ListInfo<Employee>>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new ListInfo<Employee>
            {
                Count = 2006,
                Data =
                [
                    new Employee
                    {
                        EmployeeId = new Guid("7382fb37-c489-4791-9c20-77b4f477a672"),
                        FirstName = "Will",
                        LastName = "Smith",
                        CompanyId = new Guid("ab659d2d-2df5-40be-b8ea-26fadad67d6a")
                    },
                    new Employee
                    {
                        EmployeeId = new Guid("0cb75c06-b7db-4efd-9362-9d33ab064e96"),
                        FirstName = "Jack",
                        LastName = "Parker",
                        CompanyId = new Guid("6b771327-c664-4d70-8893-ecceb2c9a9ba")
                    },
                    new Employee
                    {
                        EmployeeId = new Guid("b18310d8-da77-4739-b013-41622b7816a3"),
                        FirstName = "Justyn",
                        LastName = "Ruecker",
                        CompanyId = new Guid("6b771327-c664-4d70-8893-ecceb2c9a9ba")
                    }
                ]
            }
        );
    }
}
