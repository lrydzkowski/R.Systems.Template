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
                    ErrorMessage = "Company with the given id doesn't exist ('f639a54b-b4d0-41bf-bb1d-5d5e724cd8c0').",
                    AttemptedValue = "f639a54b-b4d0-41bf-bb1d-5d5e724cd8c0",
                    ErrorCode = "NotExist"
                }
            }
        );
    }
}
