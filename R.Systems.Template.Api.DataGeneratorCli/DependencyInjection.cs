using CommandDotNet;
using CommandDotNet.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.DataGeneratorCli.Commands;
using R.Systems.Template.Api.DataGeneratorCli.Services;
using CompanyServiceSqlServer = R.Systems.Template.Api.DataGeneratorCli.Services.SqlServer.CompanyService;
using CompanyServicePostgreSql = R.Systems.Template.Api.DataGeneratorCli.Services.PostgreSql.CompanyService;

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
        services.AddScoped<GenerateElementsCommand>();
        services.AddScoped<ElementService>();
        services.AddScoped(typeof(IVersionedServiceFactory<>), typeof(VersionedServiceFactory<>));
        services.AddScoped<ICompanyService, CompanyServicePostgreSql>();
        services.AddScoped<CompanyServicePostgreSql>();
        services.AddScoped<ICompanyService, CompanyServiceSqlServer>();
    }
}
