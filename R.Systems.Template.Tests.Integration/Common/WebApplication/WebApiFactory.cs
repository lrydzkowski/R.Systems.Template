using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db;
using R.Systems.Template.Persistence.Db.Common.Options;
using R.Systems.Template.Tests.Integration.Common.Db;
using R.Systems.Template.WebApi;

namespace R.Systems.Template.Tests.Integration.Common.WebApplication;

public class WebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
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

    private static bool _isDatabaseInitialized;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) => SetDatabaseConnectionString(configBuilder));
        builder.ConfigureTestServices(InitializeDatabase);
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

    private static void InitializeDatabase(IServiceCollection services)
    {
        if (_isDatabaseInitialized)
        {
            return;
        }

        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DbInitializer.InitializeData(dbContext);
        _isDatabaseInitialized = true;
    }
}
