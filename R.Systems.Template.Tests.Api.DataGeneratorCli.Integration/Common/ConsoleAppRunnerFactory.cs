using CommandDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.DataGeneratorCli;
using R.Systems.Template.Infrastructure.Db.Common.Options;
using RunMethodsSequentially;
using Testcontainers.PostgreSql;

namespace R.Systems.Template.Tests.Api.DataGeneratorCli.Integration.Common;

public class ConsoleAppRunnerFactory : AppRunnerFactory, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
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
                [$"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppPostgresDb)}"] =
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
        return _dbContainer.GetConnectionString();
    }
}
