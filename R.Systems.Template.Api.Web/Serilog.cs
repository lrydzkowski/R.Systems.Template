using Microsoft.ApplicationInsights.Extensibility;
using R.Systems.Template.Core.Common.Extensions;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Sinks.SystemConsole.Themes;

namespace R.Systems.Template.Api.Web;

public static class Serilog
{
    private static readonly string OutputTemplate =
        "{Timestamp:HH:mm:ss.fff zzz} [{Level}] [{SourceContext}]{NewLine}{Message:lj}{NewLine}{Exception}"
        + "-".RepeatString(120)
        + "{NewLine}";

    private static readonly ConsoleTheme Theme = AnsiConsoleTheme.Code;

    public static ReloadableLogger CreateBootstrapLogger()
    {
        return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.DefineConsoleSink()
            .WriteTo.DefineFileSink("Logs/startup-.log")
            .CreateBootstrapLogger();
    }

    public static void CreateLogger(
        HostBuilderContext context,
        IServiceProvider serviceProvider,
        LoggerConfiguration loggerConfiguration
    )
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(serviceProvider)
            .Enrich.FromLogContext()
            .WriteTo.DefineConsoleSink()
            .WriteTo.Async(
                configuration => configuration.DefineFileSink("Logs/all-.log")
            )
            .WriteTo.Async(
                configuration => configuration.DefineFileSink("Logs/errors-.log", LogEventLevel.Warning)
            )
            .WriteTo.DefineApplicationInsightsSink(
                serviceProvider
            );
    }

    private static LoggerConfiguration DefineConsoleSink(this LoggerSinkConfiguration sinkConfiguration)
    {
        return sinkConfiguration.Console(
            outputTemplate: OutputTemplate,
            theme: Theme
        );
    }

    private static LoggerConfiguration DefineFileSink(
        this LoggerSinkConfiguration sinkConfiguration,
        string path,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
    )
    {
        return sinkConfiguration.File(
            path,
            rollingInterval: RollingInterval.Day,
            fileSizeLimitBytes: 10000000,
            retainedFileCountLimit: 10,
            outputTemplate: OutputTemplate,
            restrictedToMinimumLevel: restrictedToMinimumLevel
        );
    }

    private static LoggerConfiguration DefineApplicationInsightsSink(
        this LoggerSinkConfiguration sinkConfiguration,
        IServiceProvider services
    )
    {
        return sinkConfiguration.ApplicationInsights(
            services.GetRequiredService<TelemetryConfiguration>(),
            TelemetryConverter.Traces
        );
    }
}
