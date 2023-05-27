using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.Db;
using R.Systems.Template.Infrastructure.Db.Common.Options;
using R.Systems.Template.Tests.Core.Integration.Common.Db;
using Testcontainers.MsSql;

namespace R.Systems.Template.Tests.Core.Integration.Common;

public class SystemUnderTest<TDbInitializer> : IAsyncLifetime where TDbInitializer : DbInitializerBase, new()
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
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
                [$"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppSqlServerDb)}"] =
                    BuildConnectionString()
            }
        );
    }

    private async Task InitializeDatabaseAsync(string connectionString)
    {
        await using SqlConnection connection = new(connectionString);
        await connection.OpenAsync();
        new TDbInitializer().Initialize(connection);
    }

    private string BuildConnectionString()
    {
        return _dbContainer.GetConnectionString().Replace("localhost", "127.0.0.1");
    }
}
