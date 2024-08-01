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
                        CompanyId = new Guid("ab311d67-207a-4464-a542-913357b5a0ac"),
                        Name = "Meta"
                    },
                    new Company
                    {
                        CompanyId = new Guid("9099497f-70e8-4c7c-888d-d90e208fb58d"),
                        Name = "Google"
                    },
                    new Company
                    {
                        CompanyId = new Guid("cdc8b73f-707e-4db2-bfda-85abf9fa54a4"),
                        Name = "Stiedemann LLC"
                    }
                ]
            }
        );
    }
}
