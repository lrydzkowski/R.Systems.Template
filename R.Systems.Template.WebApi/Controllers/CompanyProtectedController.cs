using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace R.Systems.Template.WebApi.Controllers;

[ApiController]
[Route("companies-protected")]
public class CompanyProtectedController : ControllerBase
{
    public CompanyProtectedController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get companies protected")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(GetCompaniesResult),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [Authorize]
    [RequiredScope("companies.write")]
    [HttpGet]
    public async Task<IActionResult> GetCompaniesProtected()
    {
        // ReSharper disable once UnusedVariable
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        GetCompaniesResult result = await Mediator.Send(new GetCompaniesQuery());

        return Ok(result.Companies);
    }
}
