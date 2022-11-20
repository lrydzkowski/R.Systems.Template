using CommandDotNet;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db;
using R.Systems.Template.Persistence.Db.DataGenerator;
using R.Systems.Template.Tests.Integration.Common.Db;

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

    public override AppRunner Create()
    {
        AddConfigurationMethods.Add(SetDatabaseConnectionString);
        ConfigureServicesMethods.Add(InitializeDatabase);

        return base.Create();
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
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DbInitializer.InitializeData(dbContext);
    }
}
