using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiController]
[Authorize]
[RequiredScope("User.Access")]
[Route("employees")]
public class EmployeeController : ControllerBase
{
    public EmployeeController(ISender mediator, IMapper mapper)
    {
        Mediator = mediator;
        Mapper = mapper;
    }

    private ISender Mediator { get; }
    private IMapper Mapper { get; }

    [SwaggerOperation(Summary = "Get the employee")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(Employee),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 404, description: "Employee doesn't exist.")]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet("{employeeId}", Name = "GetEmployee")]
    public async Task<IActionResult> GetEmployee(int employeeId)
    {
        GetEmployeeQuery query = new() { EmployeeId = employeeId };
        GetEmployeeResult result = await Mediator.Send(query);
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

    [SwaggerOperation(Summary = "Get employees")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(ListInfo<Employee>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet]
    public async Task<IActionResult> GetEmployees([FromQuery] ListRequest listRequest)
    {
        ListParameters listParameters = Mapper.Map<ListParameters>(listRequest);
        GetEmployeesResult result = await Mediator.Send(new GetEmployeesQuery { ListParameters = listParameters });

        return Ok(result.Employees);
    }

    [SwaggerOperation(Summary = "Create the employee")]
    [SwaggerResponse(
        statusCode: 201,
        description: "Employee created",
        type: typeof(Employee),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 422, type: typeof(List<ErrorInfo>), contentTypes: new[] { "application/json" })]
    [SwaggerResponse(statusCode: 500)]
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest request)
    {
        CreateEmployeeCommand command = Mapper.Map<CreateEmployeeCommand>(request);
        CreateEmployeeResult result = await Mediator.Send(command);

        return CreatedAtAction(
            nameof(GetEmployee),
            new { employeeId = result.CreatedEmployee.EmployeeId },
            result.CreatedEmployee
        );
    }

    [SwaggerOperation(Summary = "Update the employee")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Employee updated",
        type: typeof(Employee),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 422, type: typeof(List<ErrorInfo>), contentTypes: new[] { "application/json" })]
    [SwaggerResponse(statusCode: 500)]
    [HttpPut("{employeeId}")]
    public async Task<IActionResult> UpdateEmployee(int employeeId, UpdateEmployeeRequest request)
    {
        UpdateEmployeeCommand command = Mapper.Map<UpdateEmployeeCommand>(request);
        command.EmployeeId = employeeId;
        UpdateEmployeeResult result = await Mediator.Send(command);

        return Ok(result.UpdatedEmployee);
    }
}
