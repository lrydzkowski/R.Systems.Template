using System.Net;
using System.Net.Mime;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using R.Systems.Template.Api.Web.Auth;
using R.Systems.Template.Api.Web.Mappers;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Api.Web.Swagger;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiController]
public class AppController : ControllerBase
{
    public AppController(ISender mediator, HealthCheckService healthCheckService)
    {
        Mediator = mediator;
        HealthCheckService = healthCheckService;
    }

    private ISender Mediator { get; }
    private HealthCheckService HealthCheckService { get; }

    [SwaggerOperation(Summary = "Get basic information about application")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(GetAppInfoResponse),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
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

    [SwaggerResponse(
        StatusCodes.Status200OK,
        ContentTypes = [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(
        StatusCodes.Status503ServiceUnavailable,
        ContentTypes = [MediaTypeNames.Application.Json]
    )]
    [SwaggerHeaderParameter(ApiKeyAuthenticationHandler.ApiKeyHeaderName)]
    [HttpGet("health")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationSchemeOptions.Name)]
    public async Task<IActionResult> Get()
    {
        HealthReport report = await HealthCheckService.CheckHealthAsync();

        return report.Status == HealthStatus.Healthy
            ? Ok(report)
            : StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
    }
}
