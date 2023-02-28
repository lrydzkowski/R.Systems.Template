using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.Db.Postgres;
using R.Systems.Template.Infrastructure.Db.Postgres.Common.Options;
using R.Systems.Template.Tests.Core.Integration.Common.Db;

namespace R.Systems.Template.Tests.Core.Integration.Common;

public class SystemUnderTest<TDbInitializer> : IAsyncLifetime where TDbInitializer : DbInitializerBase, new()
{
    // TODO: https://github.com/testcontainers/testcontainers-dotnet/issues/750#issuecomment-1412257694
#pragma warning disable CS0618
    private readonly PostgreSqlTestcontainer _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
#pragma warning restore CS0618
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
        await InitializeDatabaseAsync(_dbContainer.ConnectionString);
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    public ServiceProvider BuildServiceProvider(
        Action<IServiceCollection>? configureServices = null,
        Action<IConfigurationBuilder>? setConfiguration = null
    )
    {
        IConfiguration configuration = BuildConfiguration(setConfiguration);

        IServiceCollection services = new ServiceCollection();
        services.ConfigureCoreServices();
        services.ConfigureInfrastructureDbPostgresServices(configuration);
        services.ConfigureInfrastructureAzureServices(configuration);
        configureServices?.Invoke(services);

        return services.BuildServiceProvider();
    }

    private IConfiguration BuildConfiguration(Action<IConfigurationBuilder>? setConfiguration = null)
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        setConfiguration?.Invoke(configurationBuilder);
        SetDatabaseConnectionString(configurationBuilder);

        return configurationBuilder.Build();
    }

    private void SetDatabaseConnectionString(IConfigurationBuilder configBuilder)
    {
        configBuilder.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                [$"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppDb)}"] =
                    _dbContainer.ConnectionString
            }
        );
    }

    private async Task InitializeDatabaseAsync(string connectionString)
    {
        await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();
        await new TDbInitializer().InitializeAsync(connection);
    }
}
