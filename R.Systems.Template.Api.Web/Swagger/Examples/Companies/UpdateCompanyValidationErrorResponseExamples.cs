using R.Systems.Template.Core.Common.Errors;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Companies;

public class UpdateCompanyValidationErrorResponseExamples : IMultipleExamplesProvider<List<ErrorInfo>>
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
                    PropertyName = "Name",
                    ErrorMessage = "'Name' must not be empty.",
                    AttemptedValue = " ",
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
                    PropertyName = "Name",
                    ErrorMessage = "The length of 'Name' must be 200 characters or fewer. You entered 207 characters.",
                    AttemptedValue =
                        "222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222",
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
                    ErrorMessage = "Company with the given id doesn't exist ('1cfe188a-ffdc-4f1e-83bd-31467b57bca3').",
                    AttemptedValue = "1cfe188a-ffdc-4f1e-83bd-31467b57bca3",
                    ErrorCode = "NotExist"
                }
            }
        );
    }
}
