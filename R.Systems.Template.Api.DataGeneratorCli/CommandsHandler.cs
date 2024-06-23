using CommandDotNet;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.DataGeneratorCli.Commands;

namespace R.Systems.Template.Api.DataGeneratorCli;

internal class CommandsHandler
{
    private readonly ILogger<CommandsHandler> _logger;

    public CommandsHandler(ILogger<CommandsHandler> logger)
    {
        _logger = logger;
    }

    [Subcommand] public GenerateCommand? GenerateCommand { get; set; }

    [Subcommand] public GetCommand? GetCommand { get; set; }

    public async Task<int> Interceptor(InterceptorExecutionDelegate next, CommandContext _)
    {
        int result = 1;
        try
        {
            result = await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error has occurred.");
        }

        return result;
    }
}
