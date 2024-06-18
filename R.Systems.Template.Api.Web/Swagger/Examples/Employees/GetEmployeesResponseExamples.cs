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
                        EmployeeId = 1,
                        FirstName = "Will",
                        LastName = "Smith",
                        CompanyId = 5
                    },
                    new Employee
                    {
                        EmployeeId = 2,
                        FirstName = "Jack",
                        LastName = "Parker",
                        CompanyId = 7
                    },
                    new Employee
                    {
                        EmployeeId = 3,
                        FirstName = "Justyn",
                        LastName = "Ruecker",
                        CompanyId = 9
                    }
                ]
            }
        );
    }
}
