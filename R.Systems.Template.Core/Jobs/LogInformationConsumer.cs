using MassTransit;
using Microsoft.Extensions.Logging;

namespace R.Systems.Template.Core.Jobs;

public class LogInformationConsumer : IConsumer<LogInformation>
{
    private readonly ILogger<LogInformationConsumer> _logger;

    public LogInformationConsumer(ILogger<LogInformationConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<LogInformation> context)
    {
        Thread.Sleep(TimeSpan.FromMinutes(1));
        _logger.LogInformation("It works :)");
        return Task.CompletedTask;
    }
}

public record LogInformation;
