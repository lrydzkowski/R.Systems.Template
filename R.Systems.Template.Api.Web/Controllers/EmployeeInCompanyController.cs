using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Api.Web.Mappers;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[RequiredScope("User.Access")]
[Route("companies")]
public class EmployeeInCompanyController : ControllerBase
{
    public EmployeeInCompanyController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get the employee in the company")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(Employee),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 404, description: "Employee doesn't exist.")]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet("{companyId}/employees/{employeeId}", Name = "GetEmployeeInCompany")]
    public async Task<IActionResult> GetEmployeeInCompany(
        int? companyId,
        int? employeeId,
        CancellationToken cancellationToken
    )
    {
        GetEmployeeQuery query = new() { CompanyId = companyId, EmployeeId = employeeId };
        GetEmployeeResult result = await Mediator.Send(query, cancellationToken);
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
        statusCode: 200,
        description: "Correct response",
        type: typeof(ListInfo<Employee>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet("{companyId}/employees", Name = "GetEmployeesInCompany")]
    public async Task<IActionResult> GetEmployeesInCompany(
        [FromQuery] ListRequest listRequest,
        int? companyId,
        CancellationToken cancellationToken
    )
    {
        ListMapper mapper = new();
        ListParameters listParameters = mapper.ToListParameter(listRequest);
        GetEmployeesResult result = await Mediator.Send(
            new GetEmployeesQuery { ListParameters = listParameters, CompanyId = companyId },
            cancellationToken
        );

        return Ok(result.Employees);
    }
}
