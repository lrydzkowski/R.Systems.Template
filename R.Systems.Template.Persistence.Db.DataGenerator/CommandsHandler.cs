﻿using CommandDotNet;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Persistence.Db.DataGenerator.Commands;

namespace R.Systems.Template.Persistence.Db.DataGenerator;

internal class CommandsHandler
{
    public CommandsHandler(ILogger<CommandsHandler> logger)
    {
        Logger = logger;
    }

    private ILogger<CommandsHandler> Logger { get; }

    [Subcommand]
    public GenerateCommand? GenerateCommand { get; set; }

    public async Task<int> Interceptor(InterceptorExecutionDelegate next, CommandContext _)
    {
        int result = 1;
        try
        {
            result = await next();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An unexpected error has occurred.");
        }

        return result;
    }
}
