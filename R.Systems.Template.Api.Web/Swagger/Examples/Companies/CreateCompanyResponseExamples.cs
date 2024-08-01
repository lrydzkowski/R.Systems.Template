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
                CompanyId = new Guid("3b9a1001-30df-447d-9461-449eecca3d65"),
                Name = "Meta"
            }
        );
    }
}
