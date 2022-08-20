using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.WebApi.Controllers;

[ApiController]
[Route("companies")]
public class CompanyController : ControllerBase
{
    public CompanyController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get company")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(GetCompanyResult),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 404, description: "Company doesn't exist.")]
    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetCompany(int companyId)
    {
        GetCompanyResult result = await Mediator.Send(new GetCompanyQuery { CompanyId = companyId });
        if (result.Company == null)
        {
            return NotFound(null);
        }

        return Ok(result.Company);
    }
}
