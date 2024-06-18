using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Companies;

public class GetCompaniesResponseExamples : IMultipleExamplesProvider<ListInfo<Company>>
{
    public IEnumerable<SwaggerExample<ListInfo<Company>>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new ListInfo<Company>
            {
                Count = 2006,
                Data =
                [
                    new Company
                    {
                        CompanyId = 1,
                        Name = "Meta"
                    },
                    new Company
                    {
                        CompanyId = 2,
                        Name = "Google"
                    },
                    new Company
                    {
                        CompanyId = 9,
                        Name = "Stiedemann LLC"
                    }
                ]
            }
        );
    }
}
