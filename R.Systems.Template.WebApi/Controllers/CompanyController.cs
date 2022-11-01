using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Validation;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.WebApi.Api;
using R.Systems.Template.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.WebApi.Controllers;

[ApiController]
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
    public async Task<IActionResult> GetCompany(int companyId)
    {
        GetCompanyQuery query = new() { CompanyId = companyId };
        Result<Company?> result = await Mediator.Send(query);
        ErrorInfo notFoundError = new ErrorInfo
        {
            PropertyName = "Company",
            ErrorMessage = "Company doesn't exist.",
            ErrorCode = "NotExist",
            AttemptedValue = query
        };

        return result.ToOkOrNotFound(notFoundError);
    }

    [SwaggerOperation(Summary = "Get companies")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(List<Company>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet]
    public async Task<IActionResult> GetCompanies([FromQuery] ListRequest listRequest)
    {
        ListParameters listParameters = Mapper.Map<ListParameters>(listRequest);
        Result<List<Company>> result = await Mediator.Send(new GetCompaniesQuery { ListParameters = listParameters });

        return result.ToOk();
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
        Result<Company> result = await Mediator.Send(command);

        return result.ToActionResult(
            company => new CreatedAtActionResult(
                nameof(GetCompany),
                this.GetControllerName(),
                new { companyId = company?.CompanyId },
                company
            )
        );
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
        Result<Company> result =
            await Mediator.Send(new UpdateCompanyCommand { CompanyId = companyId, Name = request.Name });

        return result.ToOk();
    }
}
