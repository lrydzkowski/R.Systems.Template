using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiController]
[Authorize]
[RequiredScope("User.Access")]
[Route("companies")]
public class CompanyController : ControllerBase
{
    public CompanyController(ISender mediator, IMapper mapper)
    {
        Mediator = mediator;
        Mapper = mapper;
    }

    private ISender Mediator { get; }
    private IMapper Mapper { get; }

    [SwaggerOperation(Summary = "Get the company")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(Company),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 404, description: "Company doesn't exist.")]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet("{companyId}", Name = "GetCompany")]
    public async Task<IActionResult> GetCompany(int companyId, CancellationToken cancellationToken)
    {
        GetCompanyQuery query = new() { CompanyId = companyId };
        GetCompanyResult result = await Mediator.Send(query, cancellationToken);
        if (result.Company == null)
        {
            return NotFound(
                new ErrorInfo
                {
                    PropertyName = "Company",
                    ErrorMessage = "Company doesn't exist.",
                    ErrorCode = "NotExist",
                    AttemptedValue = query
                }
            );
        }

        return Ok(result.Company);
    }

    [SwaggerOperation(Summary = "Get companies")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(ListInfo<Company>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] ListRequest listRequest,
        CancellationToken cancellationToken
    )
    {
        ListParameters listParameters = Mapper.Map<ListParameters>(listRequest);
        GetCompaniesResult result = await Mediator.Send(
            new GetCompaniesQuery { ListParameters = listParameters },
            cancellationToken
        );

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
    public async Task<IActionResult> CreateCompany(CreateCompanyRequest request)
    {
        CreateCompanyCommand command = Mapper.Map<CreateCompanyCommand>(request);
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
        UpdateCompanyCommand command = Mapper.Map<UpdateCompanyCommand>(request);
        command.CompanyId = companyId;
        UpdateCompanyResult result = await Mediator.Send(command);

        return Ok(result.Company);
    }
}
