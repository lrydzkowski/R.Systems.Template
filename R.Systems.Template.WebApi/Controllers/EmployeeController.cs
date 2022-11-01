using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Validation;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.WebApi.Api;
using R.Systems.Template.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.WebApi.Controllers;

[ApiController]
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
        Result<Employee?> result = await Mediator.Send(query);
        ErrorInfo notFoundError = new ErrorInfo
        {
            PropertyName = "Employee",
            ErrorMessage = "Employee doesn't exist.",
            ErrorCode = "NotExist",
            AttemptedValue = query
        };

        return result.ToOkOrNotFound(notFoundError);
    }

    [SwaggerOperation(Summary = "Get employees")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(List<Employee>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet]
    public async Task<IActionResult> GetEmployees([FromQuery] ListRequest listRequest)
    {
        ListParameters listParameters = Mapper.Map<ListParameters>(listRequest);
        Result<List<Employee>> result = await Mediator.Send(new GetEmployeesQuery { ListParameters = listParameters });

        return result.ToOk();
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
    public async Task<IActionResult> CreateEmployee(CreateEmployeeCommand command)
    {
        Result<Employee> result = await Mediator.Send(command);

        return result.ToActionResult(
            employee => new CreatedAtActionResult(
                nameof(GetEmployee),
                this.GetControllerName(),
                new { employeeId = employee?.EmployeeId },
                employee
            )
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
        Result<Employee> result =
            await Mediator.Send(
                new UpdateEmployeeCommand
                {
                    EmployeeId = employeeId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CompanyId = request.CompanyId
                }
            );

        return result.ToOk();
    }
}
