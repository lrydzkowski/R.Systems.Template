using CommandDotNet;
using CommandDotNet.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.DataGeneratorCli.Commands;
using R.Systems.Template.Api.DataGeneratorCli.Services;

namespace R.Systems.Template.Api.DataGeneratorCli;

internal static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole());
        services.AddScoped<IConsole, SystemConsole>();
        services.AddScoped<CommandsHandler>();
        services.AddScoped<GenerateCommand>();
        services.AddScoped<GenerateCompaniesCommand>();
        services.AddScoped<GetCommand>();
        services.AddScoped<GetCompaniesCommand>();
        services.AddScoped<CompanyService>();
        services.AddScoped<GenerateElementsCommand>();
        services.AddScoped<ElementService>();
    }
}
