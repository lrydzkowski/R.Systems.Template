using R.Systems.Template.Core.Common.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Employees;

public class CreateEmployeeResponseExamples : IMultipleExamplesProvider<Employee>
{
    public IEnumerable<SwaggerExample<Employee>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new Employee
            {
                EmployeeId = new Guid("921bf007-2ee2-41b9-96c1-bd4a4871a972"),
                FirstName = "Joe",
                LastName = "Doe",
                CompanyId = new Guid("d2777f74-2450-4c60-a4c9-30b9d8e3963a")
            }
        );
    }
}
