using R.Systems.Template.Core.Common.Errors;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Companies;

public class DeleteCompanyValidationErrorResponseExamples : IMultipleExamplesProvider<List<ErrorInfo>>
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
                    PropertyName = "Company",
                    ErrorMessage = "Company with the given id doesn't exist ('51111').",
                    AttemptedValue = "51111",
                    ErrorCode = "NotExist"
                }
            }
        );
    }
}
