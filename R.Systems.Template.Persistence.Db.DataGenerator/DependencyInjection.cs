using CommandDotNet;
using CommandDotNet.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Persistence.Db.DataGenerator.Commands;
using R.Systems.Template.Persistence.Db.DataGenerator.Services;

namespace R.Systems.Template.Persistence.Db.DataGenerator;

internal static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole());
        services.AddScoped<IConsole, SystemConsole>();
        services.AddScoped<CommandsHandler>();
        services.AddScoped<GenerateCommand>();
        services.AddScoped<GenerateCompaniesCommand>();
        services.AddScoped<CompanyService>();
    }
}
