using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Api.Web.Mappers;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[RequiredScope("User.Access")]
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
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(Company),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status404NotFound, description: "Company doesn't exist.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
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
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(ListInfo<Company>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] ListRequest listRequest,
        CancellationToken cancellationToken
    )
    {
        ListMapper mapper = new();
        ListParameters listParameters = mapper.ToListParameter(listRequest);
        GetCompaniesResult result = await Mediator.Send(
            new GetCompaniesQuery { ListParameters = listParameters },
            cancellationToken
        );

        return Ok(result.Companies);
    }

    [SwaggerOperation(Summary = "Create the company")]
    [SwaggerResponse(
        StatusCodes.Status201Created,
        description: "Company created",
        type: typeof(Company),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(
        StatusCodes.Status422UnprocessableEntity,
        type: typeof(List<ErrorInfo>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> CreateCompany(CreateCompanyRequest request)
    {
        CompanyMapper companyMapper = new();
        CreateCompanyCommand command = companyMapper.ToCreateCommand(request);
        CreateCompanyResult result = await Mediator.Send(command);

        return CreatedAtAction(
            nameof(GetCompany),
            new { companyId = result.Company.CompanyId },
            result.Company
        );
    }

    [SwaggerOperation(Summary = "Update the company")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        description: "Company updated",
        type: typeof(Company),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(
        StatusCodes.Status422UnprocessableEntity,
        type: typeof(List<ErrorInfo>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [HttpPut("{companyId}")]
    public async Task<IActionResult> UpdateCompany(int companyId, UpdateCompanyRequest request)
    {
        CompanyMapper companyMapper = new();
        UpdateCompanyCommand command = companyMapper.ToUpdateCommand(request);
        command.CompanyId = companyId;
        UpdateCompanyResult result = await Mediator.Send(command);

        return Ok(result.Company);
    }

    [SwaggerOperation(Summary = "Delete the company")]
    [SwaggerResponse(StatusCodes.Status204NoContent, description: "Company deleted")]
    [SwaggerResponse(
        StatusCodes.Status422UnprocessableEntity,
        type: typeof(List<ErrorInfo>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{companyId}")]
    public async Task<IActionResult> DeleteCompany(int companyId)
    {
        DeleteCompanyCommand command = new() { CompanyId = companyId };
        await Mediator.Send(command);

        return NoContent();
    }
}
