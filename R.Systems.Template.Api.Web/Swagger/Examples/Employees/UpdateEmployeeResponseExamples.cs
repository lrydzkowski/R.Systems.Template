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
                EmployeeId = new Guid("a57616a7-4055-4d94-8a9d-25522e0f4004"),
                FirstName = "Joe",
                LastName = "Doe",
                CompanyId = new Guid("5fc71929-f34b-44c6-829a-96225949bf44")
            }
        );
    }
}
