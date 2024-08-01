using R.Systems.Template.Core.Common.Errors;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Employees;

public class GetEmployeeNotFoundResponseExamples : IMultipleExamplesProvider<ErrorInfo>
{
    public IEnumerable<SwaggerExample<ErrorInfo>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new ErrorInfo
            {
                PropertyName = "Employee",
                ErrorMessage = "Employee doesn't exist.",
                ErrorCode = "NotExist",
                AttemptedValue = new
                {
                    EmployeeId = "56c565ae-eef7-4178-89e4-6339875d1c11"
                }
            }
        );
    }
}
