using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.Db;
using R.Systems.Template.Infrastructure.Db.Common.Options;
using R.Systems.Template.Tests.Core.Integration.Common.Db;
using Testcontainers.PostgreSql;

namespace R.Systems.Template.Tests.Core.Integration.Common;

public class SystemUnderTest<TDbInitializer> : IAsyncLifetime where TDbInitializer : DbInitializerBase, new()
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithCleanUp(true)
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await InitializeDatabaseAsync(BuildConnectionString());
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
        services.ConfigureInfrastructureDbServices(configuration);
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
                [$"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppPostgresDb)}"] =
                    BuildConnectionString()
            }
        );
    }

    private async Task InitializeDatabaseAsync(string connectionString)
    {
        await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();
        await new TDbInitializer().InitializeAsync(connection);
    }

    private string BuildConnectionString()
    {
        return _dbContainer.GetConnectionString();
    }
}
