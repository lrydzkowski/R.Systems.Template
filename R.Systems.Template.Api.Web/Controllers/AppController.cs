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
using R.Systems.Template.Api.Web.Swagger.Examples.App;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Controllers;

public class AppController : ApiControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    private readonly ISender _mediator;

    public AppController(ISender mediator, HealthCheckService healthCheckService)
    {
        _mediator = mediator;
        _healthCheckService = healthCheckService;
    }

    [SwaggerOperation(Summary = "Get basic information about application")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Correct response",
        typeof(GetAppInfoResponse),
        [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponseExample(
        StatusCodes.Status200OK,
        typeof(GetAppInfoResponseExamples)
    )]
    [HttpGet("")]
    public async Task<IActionResult> GetAppInfo()
    {
        GetAppInfoResult result =
            await _mediator.Send(new GetAppInfoQuery { AppAssembly = Assembly.GetExecutingAssembly() });
        GetAppInfoMapper mapper = new();
        GetAppInfoResponse response = mapper.ToResponse(result);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "Check health")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        ContentTypes = [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(
        StatusCodes.Status503ServiceUnavailable,
        ContentTypes = [MediaTypeNames.Application.Json]
    )]
    [SwaggerHeaderParameter(
        ApiKeyAuthenticationHandler.ApiKeyHeaderName
    )]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationSchemeOptions.Name)]
    [HttpGet("health")]
    public async Task<IActionResult> Get()
    {
        HealthReport report = await _healthCheckService.CheckHealthAsync();
        return report.Status == HealthStatus.Healthy
            ? Ok(report)
            : StatusCode(StatusCodes.Status503ServiceUnavailable, report);
    }
}
