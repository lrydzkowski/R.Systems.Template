using MassTransit;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.Jobs;

namespace R.Systems.Template.Api.Web.Controllers;

[Route("jobs")]
public class JobsController : ApiControllerBase
{
    private readonly IBus _bus;
    private readonly ILogger<JobsController> _logger;

    public JobsController(IBus bus, ILogger<JobsController> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    [HttpPost("log-information")]
    public async Task<IActionResult> RunLogInformationJob()
    {
        await _bus.Publish(new LogInformation());
        Thread.Sleep(TimeSpan.FromSeconds(20));
        _logger.LogInformation("controller :)");
        return NoContent();
    }
}
