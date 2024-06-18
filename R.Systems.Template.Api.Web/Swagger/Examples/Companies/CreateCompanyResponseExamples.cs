using R.Systems.Template.Core.Common.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Companies;

public class CreateCompanyResponseExamples : IMultipleExamplesProvider<Company>
{
    public IEnumerable<SwaggerExample<Company>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new Company
            {
                CompanyId = 1,
                Name = "Meta"
            }
        );
    }
}
