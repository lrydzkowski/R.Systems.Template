using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Npgsql;
using R.Systems.Template.Api.AzureFunctions;
using R.Systems.Template.Infrastructure.Db.Common.Options;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Function;

public class FunctionFactory<TDbInitializer> : IAsyncLifetime
    where TDbInitializer : DbInitializerBase, new()
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

    public IServiceProvider? Services { get; private set; }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        Services = new FunctionHostBuilder().WithAppConfiguration(SetDatabaseConnectionString).Build().Services;
        await InitializeDatabaseAsync(_dbContainer.ConnectionString);
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

    private async Task InitializeDatabaseAsync(string connectionString)
    {
        await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();
        await new TDbInitializer().InitializeAsync(connection);
    }
}
