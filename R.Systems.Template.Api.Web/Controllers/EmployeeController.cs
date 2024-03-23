using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Api.Web.Mappers;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.DeleteEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAdB2C)]
[RequiredScope("User.Access")]
[Route("employees")]
public class EmployeeController : ControllerBase
{
    public EmployeeController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get the employee")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(Employee),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status404NotFound, description: "Employee doesn't exist.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [HttpGet("{employeeId}", Name = "GetEmployee")]
    public async Task<IActionResult> GetEmployee(int employeeId, CancellationToken cancellationToken)
    {
        GetEmployeeQuery query = new() { EmployeeId = employeeId };
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

    [SwaggerOperation(Summary = "Get employees")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(ListInfo<Employee>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetEmployees(
        [FromQuery] ListRequest listRequest,
        CancellationToken cancellationToken
    )
    {
        ListMapper mapper = new();
        ListParameters listParameters = mapper.ToListParameter(listRequest);
        GetEmployeesResult result = await Mediator.Send(
            new GetEmployeesQuery { ListParameters = listParameters },
            cancellationToken
        );

        return Ok(result.Employees);
    }

    [SwaggerOperation(Summary = "Create the employee")]
    [SwaggerResponse(
        StatusCodes.Status201Created,
        description: "Employee created",
        type: typeof(Employee),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(
        StatusCodes.Status422UnprocessableEntity,
        type: typeof(List<ErrorInfo>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest request)
    {
        EmployeeMapper mapper = new();
        CreateEmployeeCommand command = mapper.ToCreateCommand(request);
        CreateEmployeeResult result = await Mediator.Send(command);

        return CreatedAtAction(
            nameof(GetEmployee),
            new { employeeId = result.CreatedEmployee.EmployeeId },
            result.CreatedEmployee
        );
    }

    [SwaggerOperation(Summary = "Update the employee")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        description: "Employee updated",
        type: typeof(Employee),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(
        StatusCodes.Status422UnprocessableEntity,
        type: typeof(List<ErrorInfo>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [HttpPut("{employeeId}")]
    public async Task<IActionResult> UpdateEmployee(int employeeId, UpdateEmployeeRequest request)
    {
        EmployeeMapper mapper = new();
        UpdateEmployeeCommand command = mapper.ToUpdateCommand(request);
        command.EmployeeId = employeeId;
        UpdateEmployeeResult result = await Mediator.Send(command);

        return Ok(result.UpdatedEmployee);
    }

    [SwaggerOperation(Summary = "Delete the employee")]
    [SwaggerResponse(StatusCodes.Status204NoContent, description: "Employee deleted")]
    [SwaggerResponse(
        StatusCodes.Status422UnprocessableEntity,
        type: typeof(List<ErrorInfo>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{employeeId}")]
    public async Task<IActionResult> DeleteEmployee(int employeeId)
    {
        DeleteEmployeeCommand command = new() { EmployeeId = employeeId };
        await Mediator.Send(command);

        return NoContent();
    }
}
