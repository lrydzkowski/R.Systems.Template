using R.Systems.Template.Core.Common.Errors;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Employees;

public class CreateEmployeeValidationErrorResponseExamples : IMultipleExamplesProvider<List<ErrorInfo>>
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
                },
                new()
                {
                    PropertyName = "CompanyId",
                    ErrorMessage = "'CompanyId' must not be empty.",
                    AttemptedValue = null,
                    ErrorCode = "NotEmptyValidator"
                }
            }
        );

        yield return SwaggerExample.Create(
            "Example 2",
            "Example 2",
            new List<ErrorInfo>
            {
                new()
                {
                    PropertyName = "FirstName",
                    ErrorMessage =
                        "The length of 'FirstName' must be 100 characters or fewer. You entered 104 characters.",
                    AttemptedValue =
                        "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",
                    ErrorCode = "MaximumLengthValidator"
                },
                new()
                {
                    PropertyName = "LastName",
                    ErrorMessage =
                        "The length of 'LastName' must be 100 characters or fewer. You entered 104 characters.",
                    AttemptedValue =
                        "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",
                    ErrorCode = "MaximumLengthValidator"
                }
            }
        );

        yield return SwaggerExample.Create(
            "Example 3",
            "Example 3",
            new List<ErrorInfo>
            {
                new()
                {
                    PropertyName = "Company",
                    ErrorMessage = "Company with the given id doesn't exist ('eb4bb983-4458-49b1-a543-2e826d544459').",
                    AttemptedValue = "eb4bb983-4458-49b1-a543-2e826d544459",
                    ErrorCode = "NotExist"
                }
            }
        );
    }
}
