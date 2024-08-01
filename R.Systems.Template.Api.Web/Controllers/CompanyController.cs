using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Api.Web.Mappers;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Api.Web.Swagger.Examples.Companies;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Controllers;

[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[RequiredScope("User.Access")]
[Route("companies")]
public class CompanyController : ApiControllerBase
{
    private readonly ISender _mediator;

    public CompanyController(ISender mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Get the company")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Correct response",
        typeof(Company),
        [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponseExample(
        StatusCodes.Status200OK,
        typeof(GetCompanyResponseExamples)
    )]
    [SwaggerResponse(
        StatusCodes.Status404NotFound,
        "Company doesn't exist.",
        typeof(List<ErrorInfo>),
        [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponseExample(
        StatusCodes.Status404NotFound,
        typeof(GetCompanyNotFoundResponseExamples)
    )]
    [HttpGet("{companyId}", Name = "GetCompany")]
    public async Task<IActionResult> GetCompany(
        [FromHeader(Name = Headers.Version)] string? version,
        string companyId,
        CancellationToken cancellationToken
    )
    {
        GetCompanyQuery query = new()
        {
            CompanyId = companyId,
            AppContext = new ApplicationContext(version)
        };
        GetCompanyResult result = await _mediator.Send(query, cancellationToken);
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
        "Correct response",
        typeof(ListInfo<Company>),
        [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponseExample(
        StatusCodes.Status200OK,
        typeof(GetCompaniesResponseExamples)
    )]
    [HttpGet]
    public async Task<IActionResult> GetCompanies(
        [FromHeader(Name = Headers.Version)] string? version,
        [FromQuery] ListRequest listRequest,
        CancellationToken cancellationToken
    )
    {
        ListMapper mapper = new();
        ListParametersDto listParametersDto = mapper.ToListParametersDto(listRequest);
        GetCompaniesResult result = await _mediator.Send(
            new GetCompaniesQuery
                { ListParametersDto = listParametersDto, AppContext = new ApplicationContext(version) },
            cancellationToken
        );

        return Ok(result.Companies);
    }

    [SwaggerOperation(Summary = "Create the company")]
    [SwaggerResponse(
        StatusCodes.Status201Created,
        "Company created",
        typeof(Company),
        [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponseExample(
        StatusCodes.Status201Created,
        typeof(CreateCompanyResponseExamples)
    )]
    [SwaggerResponseExample(
        StatusCodes.Status422UnprocessableEntity,
        typeof(CreateCompanyValidationErrorResponseExamples)
    )]
    [Consumes(MediaTypeNames.Application.Json)]
    [HttpPost]
    public async Task<IActionResult> CreateCompany(
        [FromHeader(Name = Headers.Version)] string? version,
        CreateCompanyRequest request
    )
    {
        CompanyMapper companyMapper = new();
        CreateCompanyCommand command = companyMapper.ToCreateCommand(request);
        command.AppContext = new ApplicationContext(version);
        CreateCompanyResult result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetCompany), new { companyId = result.Company.CompanyId }, result.Company);
    }

    [SwaggerOperation(Summary = "Update the company")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Company updated",
        typeof(Company),
        [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponseExample(
        StatusCodes.Status200OK,
        typeof(UpdateCompanyResponseExamples)
    )]
    [SwaggerResponseExample(
        StatusCodes.Status422UnprocessableEntity,
        typeof(UpdateCompanyValidationErrorResponseExamples)
    )]
    [Consumes(MediaTypeNames.Application.Json)]
    [HttpPut("{companyId}")]
    public async Task<IActionResult> UpdateCompany(
        [FromHeader(Name = Headers.Version)] string? version,
        string companyId,
        UpdateCompanyRequest request
    )
    {
        CompanyMapper companyMapper = new();
        UpdateCompanyCommand command = companyMapper.ToUpdateCommand(request);
        command.AppContext = new ApplicationContext(version);
        command.CompanyId = companyId;
        UpdateCompanyResult result = await _mediator.Send(command);

        return Ok(result.Company);
    }

    [SwaggerOperation(Summary = "Delete the company")]
    [SwaggerResponse(
        StatusCodes.Status204NoContent,
        "Company deleted"
    )]
    [SwaggerResponseExample(
        StatusCodes.Status422UnprocessableEntity,
        typeof(DeleteCompanyValidationErrorResponseExamples)
    )]
    [HttpDelete("{companyId}")]
    public async Task<IActionResult> DeleteCompany(
        [FromHeader(Name = Headers.Version)] string? version,
        string companyId
    )
    {
        DeleteCompanyCommand command = new()
        {
            CompanyId = companyId,
            AppContext = new ApplicationContext(version)
        };
        await _mediator.Send(command);

        return NoContent();
    }
}
