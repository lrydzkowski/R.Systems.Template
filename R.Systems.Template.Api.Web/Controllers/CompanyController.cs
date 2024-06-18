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
    public CompanyController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

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
    public async Task<IActionResult> UpdateCompany(int companyId, UpdateCompanyRequest request)
    {
        CompanyMapper companyMapper = new();
        UpdateCompanyCommand command = companyMapper.ToUpdateCommand(request);
        command.CompanyId = companyId;
        UpdateCompanyResult result = await Mediator.Send(command);

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
    public async Task<IActionResult> DeleteCompany(int companyId)
    {
        DeleteCompanyCommand command = new() { CompanyId = companyId };
        await Mediator.Send(command);

        return NoContent();
    }
}
