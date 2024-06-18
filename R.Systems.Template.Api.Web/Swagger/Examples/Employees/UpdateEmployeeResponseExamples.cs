using R.Systems.Template.Core.Common.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Employees;

public class UpdateEmployeeResponseExamples : IMultipleExamplesProvider<Employee>
{
    public IEnumerable<SwaggerExample<Employee>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new Employee
            {
                EmployeeId = 4,
                FirstName = "Joe",
                LastName = "Doe",
                CompanyId = 1
            }
        );
    }
}
