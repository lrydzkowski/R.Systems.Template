using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Azure;
using R.Systems.Template.Infrastructure.Db;
using R.Systems.Template.Infrastructure.Db.Common.Options;
using R.Systems.Template.Tests.Core.Integration.Common.Db;

namespace R.Systems.Template.Tests.Core.Integration.Common;

public class SystemUnderTest<TDbInitializer> : IAsyncLifetime where TDbInitializer : DbInitializerBase, new()
{
    // TODO: https://github.com/testcontainers/testcontainers-dotnet/issues/750#issuecomment-1412257694
#pragma warning disable CS0618
    private readonly MsSqlTestcontainer _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
#pragma warning restore CS0618
        .WithDatabase(
            new MsSqlTestcontainerConfiguration()
            {
                Database = "r_systems_template",
                Password = Guid.NewGuid().ToString()
            }
        )
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
                [$"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppDb)}"] =
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
        return _dbContainer.ConnectionString + ";Trust Server Certificate=true";
    }
}
