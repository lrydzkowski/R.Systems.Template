using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.WebApi.Controllers;

[ApiController]
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
    public async Task<IActionResult> GetEmployeeInCompany(int companyId, int employeeId)
    {
        GetEmployeeQuery query = new() { CompanyId = companyId, EmployeeId = employeeId };
        GetEmployeeResult result = await Mediator.Send(query);
        if (result.Employee == null)
        {
            return NotFound(new ErrorInfo
            {
                PropertyName = "Employee",
                ErrorMessage = "Employee doesn't exist.",
                ErrorCode = "NotExist",
                AttemptedValue = query
            });
        }

        return Ok(result.Employee);
    }

    [SwaggerOperation(Summary = "Get employees in the company")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(List<Employee>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet("{companyId}/employees", Name = "GetEmployeesInCompany")]
    public async Task<IActionResult> GetEmployeesInCompany(int companyId)
    {
        GetEmployeesResult result = await Mediator.Send(new GetEmployeesQuery { CompanyId = companyId });

        return Ok(result.Employees);
    }
}
