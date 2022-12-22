using CommandDotNet;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db.DataGenerator;
using RunMethodsSequentially;

namespace R.Systems.Template.Tests.Integration.Common.ConsoleAppRunner;

public class ConsoleAppRunnerFactory : AppRunnerFactory, IAsyncLifetime
{
    private readonly PostgreSqlTestcontainer _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(
            new PostgreSqlTestcontainerConfiguration
            {
                Database = "r-systems-template",
                Username = "postgres",
                Password = Guid.NewGuid().ToString()
            }
        )
        .WithImage("postgres:14-alpine")
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
                ["ConnectionStrings:AppDb"] = _dbContainer.ConnectionString
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
