using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db.Common.Options;
using R.Systems.Template.WebApi;
using RunMethodsSequentially;

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
}

public class WebApiFactoryWithDb<TDbInitializer> : WebApiFactory
    where TDbInitializer : class, IStartupServiceToRunSequentially
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(ConfigureDbInitializer);
    }

    private void ConfigureDbInitializer(IServiceCollection services)
    {
        services.RegisterRunMethodsSequentially(
                options => options.AddFileSystemLockAndRunMethods(Environment.CurrentDirectory)
            )
            .RegisterServiceToRunInJob<TDbInitializer>();
    }
}
