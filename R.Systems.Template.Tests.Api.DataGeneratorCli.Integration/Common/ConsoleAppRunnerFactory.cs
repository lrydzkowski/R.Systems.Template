using CommandDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.DataGeneratorCli;
using RunMethodsSequentially;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;
using PostgresSqlDbConnectionStringsOptions =
    R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Options.ConnectionStringsOptions;
using SqlServerDbConnectionStringsOptions =
    R.Systems.Template.Infrastructure.SqlServerDb.Common.Options.ConnectionStringsOptions;

namespace R.Systems.Template.Tests.Api.DataGeneratorCli.Integration.Common;

public class ConsoleAppRunnerFactory : AppRunnerFactory, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer =
        new PostgreSqlBuilder().WithImage("postgres:15-alpine").WithCleanUp(true).Build();

    private readonly MsSqlContainer _sqlServerContainer = new MsSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await _sqlServerContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
        await _sqlServerContainer.DisposeAsync();
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
                [$"{PostgresSqlDbConnectionStringsOptions.Position}:{nameof(PostgresSqlDbConnectionStringsOptions.AppPostgreSqlDb)}"] =
                    _postgreSqlContainer.GetConnectionString(),
                [$"{SqlServerDbConnectionStringsOptions.Position}:{nameof(SqlServerDbConnectionStringsOptions.AppSqlServerDb)}"] =
                    _sqlServerContainer.GetConnectionString()
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
}
