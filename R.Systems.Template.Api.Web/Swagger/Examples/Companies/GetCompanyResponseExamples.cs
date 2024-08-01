using R.Systems.Template.Core.Common.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Companies;

public class GetCompanyResponseExamples : IMultipleExamplesProvider<Company>
{
    public IEnumerable<SwaggerExample<Company>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new Company
            {
                CompanyId = new Guid("917b01e7-9f15-440a-830f-e384da4df97b"),
                Name = "Meta"
            }
        );
    }
}
