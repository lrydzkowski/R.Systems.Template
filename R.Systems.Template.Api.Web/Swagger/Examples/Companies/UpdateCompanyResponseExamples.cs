using R.Systems.Template.Core.Common.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Companies;

public class UpdateCompanyResponseExamples : IMultipleExamplesProvider<Company>
{
    public IEnumerable<SwaggerExample<Company>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new Company
            {
                CompanyId = new Guid("53f8abb3-3844-47ee-adc0-ea1ebe66d69b"),
                Name = "Meta"
            }
        );
    }
}
