using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
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

    [HttpGet, Route("")]
    public async Task<IActionResult> GetAppInfo()
    {
        GetAppInfoResult getAppInfoResult = await Mediator.Send(
            new GetAppInfoQuery { AppAssembly = Assembly.GetExecutingAssembly() }
        );

        return Ok(getAppInfoResult);
    }
}
