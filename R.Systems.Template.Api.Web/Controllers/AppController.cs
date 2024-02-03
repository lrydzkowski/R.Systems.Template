using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Api.Web.Mappers;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiController]
public class AppController : ControllerBase
{
    private readonly ILogger<AppController> _logger;

    public AppController(ISender mediator, ILogger<AppController> logger)
    {
        _logger = logger;
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get basic information about application")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(GetAppInfoResponse),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet, Route("")]
    public async Task<IActionResult> GetAppInfo()
    {
        GetAppInfoResult result = await Mediator.Send(
            new GetAppInfoQuery { AppAssembly = Assembly.GetExecutingAssembly() }
        );
        GetAppInfoMapper mapper = new();
        GetAppInfoResponse response = mapper.ToResponse(result);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "Save logs")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpPost, Route("logs")]
    public IActionResult SaveLogs()
    {
        _logger.LogTrace("It's trace with a {placeholder}", "test placeholder");
        _logger.LogDebug("It's debug with a {placeholder}", "test placeholder");
        _logger.LogInformation("It's information with a {placeholder}", "test placeholder");
        _logger.LogWarning("It's warning with a {placeholder}", "test placeholder");
        _logger.LogError("It's error with a {placeholder}", "test placeholder");
        _logger.LogCritical("It's critical with a {placeholder}", "test placeholder");

        return Ok();
    }
}
