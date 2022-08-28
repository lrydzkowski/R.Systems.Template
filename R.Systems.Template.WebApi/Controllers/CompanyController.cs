using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.WebApi.Api;
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

    [SwaggerOperation(Summary = "Get the company")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(GetCompanyResult),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 404, description: "Company doesn't exist.")]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet("{companyId}", Name = "GetCompany")]
    public async Task<IActionResult> GetCompany(int companyId)
    {
        GetCompanyQuery query = new() { CompanyId = companyId };
        GetCompanyResult result = await Mediator.Send(query);
        if (result.Company == null)
        {
            return NotFound(new ErrorInfo
            {
                PropertyName = "Company",
                ErrorMessage = "Company doesn't exist.",
                ErrorCode = "NotExist",
                AttemptedValue = query
            });
        }

        return Ok(result.Company);
    }

    [SwaggerOperation(Summary = "Get companies")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(GetCompaniesResult),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        GetCompaniesResult result = await Mediator.Send(new GetCompaniesQuery());

        return Ok(result.Companies);
    }

    [SwaggerOperation(Summary = "Create the company")]
    [SwaggerResponse(
        statusCode: 201,
        description: "Company created",
        type: typeof(Company),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 422, type: typeof(List<ErrorInfo>), contentTypes: new[] { "application/json" })]
    [SwaggerResponse(statusCode: 500)]
    [HttpPost]
    public async Task<IActionResult> CreateCompany(CreateCompanyCommand command)
    {
        CreateCompanyResult result = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetCompany), new { companyId = result.Company.CompanyId }, result.Company);
    }

    [SwaggerOperation(Summary = "Update the company")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Company updated",
        type: typeof(Company),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 422, type: typeof(List<ErrorInfo>), contentTypes: new[] { "application/json" })]
    [SwaggerResponse(statusCode: 500)]
    [HttpPut("{companyId}")]
    public async Task<IActionResult> UpdateCompany(int companyId, UpdateCompanyRequest request)
    {
        UpdateCompanyResult result =
            await Mediator.Send(new UpdateCompanyCommand { CompanyId = companyId, Name = request.Name });

        return Ok(result.Company);
    }
}
