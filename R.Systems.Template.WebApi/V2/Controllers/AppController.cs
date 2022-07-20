using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.WebApi.V2.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

namespace R.Systems.Template.WebApi.V2.Controllers;

[ApiController]
[ApiVersion("2.0")]
[ApiExplorerSettings(GroupName = "v2")]
[Route("v{version:apiVersion}")]
public class AppController : ControllerBase
{
    public AppController(ISender mediator, IMapper mapper)
    {
        Mediator = mediator;
        Mapper = mapper;
    }

    private ISender Mediator { get; }
    private IMapper Mapper { get; }

    [SwaggerOperation(Summary = "Get basic information about application")]
    [SwaggerResponse(statusCode: 200, type: typeof(GetAppInfoResponse), contentTypes: new[] { "application/json" })]
    [HttpGet, Route("")]
    public async Task<IActionResult> GetAppInfo()
    {
        GetAppInfoResult result = await Mediator.Send(
            new GetAppInfoQuery { AppAssembly = Assembly.GetExecutingAssembly() }
        );
        GetAppInfoResponse response = Mapper.Map<GetAppInfoResponse>(result);

        return Ok(response);
    }
}
