using R.Systems.Template.Api.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.App;

public class GetAppInfoResponseExamples : IMultipleExamplesProvider<GetAppInfoResponse>
{
    public IEnumerable<SwaggerExample<GetAppInfoResponse>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new GetAppInfoResponse
            {
                AppName = "R.Systems.Template.Api.Web",
                AppVersion = "1.0.0-preview.1+c7e791e6c495cec50ec8f3f9c2ff1d261d70b3fb"
            }
        );
    }
}
