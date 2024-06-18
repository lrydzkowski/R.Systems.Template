using R.Systems.Template.Core.Common.Errors;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Employees;

public class UpdateEmployeeValidationErrorResponseExamples : IMultipleExamplesProvider<List<ErrorInfo>>
{
    public IEnumerable<SwaggerExample<List<ErrorInfo>>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new List<ErrorInfo>
            {
                new()
                {
                    PropertyName = "FirstName",
                    ErrorMessage = "'FirstName' must not be empty.",
                    AttemptedValue = "",
                    ErrorCode = "NotEmptyValidator"
                },
                new()
                {
                    PropertyName = "LastName",
                    ErrorMessage = "'LastName' must not be empty.",
                    AttemptedValue = "",
                    ErrorCode = "NotEmptyValidator"
                }
            }
        );
    }
}
