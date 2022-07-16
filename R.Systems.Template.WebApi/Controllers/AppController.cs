using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

namespace R.Systems.Template.WebApi.Controllers;

[ApiController]
public class AppController : ControllerBase
{
    public AppController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get basic information about application")]
    [SwaggerResponse(statusCode: 200, type: typeof(GetAppInfoResult), contentTypes: new[] { "application/json" })]
    [HttpGet, Route("")]
    public async Task<IActionResult> GetAppInfo()
    {
        GetAppInfoResult getAppInfoResult = await Mediator.Send(
            new GetAppInfoQuery { AppAssembly = Assembly.GetExecutingAssembly() }
        );

        return Ok(getAppInfoResult);
    }
}
