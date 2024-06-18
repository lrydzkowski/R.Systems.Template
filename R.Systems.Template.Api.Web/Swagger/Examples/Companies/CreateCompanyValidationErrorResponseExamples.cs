using R.Systems.Template.Core.Common.Errors;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Companies;

public class CreateCompanyValidationErrorResponseExamples : IMultipleExamplesProvider<List<ErrorInfo>>
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
                    ErrorMessage = "Company with the given name already exists ('123').",
                    AttemptedValue = "123",
                    ErrorCode = "UniquenessValidator"
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
                    ErrorMessage = "'Name' must not be empty.",
                    AttemptedValue = " ",
                    ErrorCode = "NotEmptyValidator"
                }
            }
        );
    }
}
