using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
using R.Systems.Template.Api.AzureFunctions;
using R.Systems.Template.Infrastructure.Db.Common.Options;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Function;

public class FunctionFactory<TDbInitializer> : IAsyncLifetime
    where TDbInitializer : DbInitializerBase, new()
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

    public IServiceProvider? Services { get; private set; }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        Services = new FunctionHostBuilder().WithAppConfiguration(SetDatabaseConnectionString).Build().Services;
        await InitializeDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
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

    private async Task InitializeDatabaseAsync()
    {
        await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(_dbContainer.ConnectionString);
        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();
        await new TDbInitializer().InitializeAsync(connection);
    }
}
