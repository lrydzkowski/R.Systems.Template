using CommandDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.DataGeneratorCli;
using R.Systems.Template.Infrastructure.Db.Common.Options;
using RunMethodsSequentially;
using Testcontainers.MsSql;

namespace R.Systems.Template.Tests.Api.DataGeneratorCli.Integration.Common;

public class ConsoleAppRunnerFactory : AppRunnerFactory, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
        .WithCleanUp(true)
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    public override async Task<AppRunner> CreateAsync()
    {
        AddConfigurationMethods.Add(SetDatabaseConnectionString);
        ConfigureServicesMethods.Add(InitializeDatabase);

        return await base.CreateAsync();
    }

    private void SetDatabaseConnectionString(IConfigurationBuilder configBuilder)
    {
        configBuilder.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                [$"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppSqlServerDb)}"] =
                    BuildConnectionString()
            }
        );
    }

    private static void InitializeDatabase(IServiceCollection services)
    {
        services.RegisterRunMethodsSequentially(
                options =>
                {
                    options.RegisterAsHostedService = false;
                    options.AddFileSystemLockAndRunMethods(Environment.CurrentDirectory);
                }
            )
            .RegisterServiceToRunInJob<ConsoleSampleDataDbInitializer>();
    }

    private string BuildConnectionString()
    {
        return _dbContainer.GetConnectionString().Replace("localhost", "127.0.0.1");
    }
}
