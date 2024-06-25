using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Api.Web.Mappers;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.Api.Web.Controllers;

[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[RequiredScope("User.Access")]
[Route("companies")]
public class EmployeeInCompanyController : ApiControllerBase
{
    private readonly ISender _mediator;

    public EmployeeInCompanyController(ISender mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Get the employee in the company")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Correct response",
        typeof(Employee),
        [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(
        StatusCodes.Status404NotFound,
        "Employee doesn't exist."
    )]
    [HttpGet("{companyId}/employees/{employeeId}", Name = "GetEmployeeInCompany")]
    public async Task<IActionResult> GetEmployeeInCompany(
        [FromHeader(Name = Headers.Version)] string? version,
        int companyId,
        int employeeId,
        CancellationToken cancellationToken
    )
    {
        GetEmployeeQuery query = new()
        {
            CompanyId = companyId,
            EmployeeId = employeeId,
            AppContext = new ApplicationContext(version)
        };
        GetEmployeeResult result = await _mediator.Send(query, cancellationToken);
        if (result.Employee == null)
        {
            return NotFound(
                new ErrorInfo
                {
                    PropertyName = "Employee",
                    ErrorMessage = "Employee doesn't exist.",
                    ErrorCode = "NotExist",
                    AttemptedValue = query
                }
            );
        }

        return Ok(result.Employee);
    }

    [SwaggerOperation(Summary = "Get employees in the company")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Correct response",
        typeof(ListInfo<Employee>),
        [MediaTypeNames.Application.Json]
    )]
    [HttpGet("{companyId}/employees", Name = "GetEmployeesInCompany")]
    public async Task<IActionResult> GetEmployeesInCompany(
        [FromHeader(Name = Headers.Version)] string? version,
        [FromQuery] ListRequest listRequest,
        int companyId,
        CancellationToken cancellationToken
    )
    {
        ListMapper mapper = new();
        ListParameters listParameters = mapper.ToListParameter(listRequest);
        GetEmployeesResult result = await _mediator.Send(
            new GetEmployeesQuery
            {
                ListParameters = listParameters, CompanyId = companyId, AppContext = new ApplicationContext(version)
            },
            cancellationToken
        );

        return Ok(result.Employees);
    }
}
