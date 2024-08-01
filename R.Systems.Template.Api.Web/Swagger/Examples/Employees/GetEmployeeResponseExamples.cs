using R.Systems.Template.Core.Common.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Employees;

public class GetEmployeeResponseExamples : IMultipleExamplesProvider<Employee>
{
    public IEnumerable<SwaggerExample<Employee>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new Employee
            {
                EmployeeId = new Guid("4bbab7ac-9deb-44cf-9fe3-83e5b1208524"),
                FirstName = "John",
                LastName = "Doe",
                CompanyId = new Guid("671ebcfd-472a-41f9-b733-64631cb69044")
            }
        );
    }
}
