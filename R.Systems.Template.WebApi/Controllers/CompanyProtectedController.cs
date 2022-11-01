using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.WebApi.Extensions;
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
        type: typeof(List<Company>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [Authorize]
    [RequiredScope("User.Access")]
    [HttpGet]
    public async Task<IActionResult> GetCompaniesProtected()
    {
        // ReSharper disable once UnusedVariable
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        string? email = User.FindFirstValue(ClaimTypes.Email);

        Result<List<Company>> result = await Mediator.Send(new GetCompaniesQuery());

        return result.ToOk();
    }
}
