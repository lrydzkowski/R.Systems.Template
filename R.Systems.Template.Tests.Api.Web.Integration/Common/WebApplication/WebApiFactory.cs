using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.Web;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Assertion;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAd;
using R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAdB2C;
using R.Systems.Template.Tests.Api.Web.Integration.Options.ConnectionStrings;
using R.Systems.Template.Tests.Api.Web.Integration.Options.HealthCheck;
using R.Systems.Template.Tests.Api.Web.Integration.Options.Serilog;
using R.Systems.Template.Tests.Api.Web.Integration.Options.Wordnik;
using RunMethodsSequentially;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using WireMock.Server;
using DbConnectionStringsOptions = R.Systems.Template.Infrastructure.Db.Common.Options.ConnectionStringsOptions;
using MongoDbConnectionStringsOptions =
    R.Systems.Template.Infrastructure.MongoDb.Common.Options.ConnectionStringsOptions;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;

public class WebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string MongoDbName = "admin";

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithCleanUp(true)
        .Build();

    private readonly List<IOptionsData> _defaultOptionsData = new()
    {
        new AzureAdOptionsData(), new AzureAdB2COptionsData(), new ConnectionStringsOptionsData(),
        new WordnikOptionsData(), new HealthCheckOptionsData(), new SerilogOptionsData()
    };

    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().WithImage("mongo:6.0").Build();

    public WebApiFactory()
    {
        WireMockServer = WireMockServer.Start();
        AssertionOptions.AssertEquivalencyUsing(options => options.Using(new JsonElementEquivalencyStep()));
    }

    public WireMockServer WireMockServer { get; }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _mongoDbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        WireMockServer.Dispose();
        await _dbContainer.DisposeAsync();
        await _mongoDbContainer.DisposeAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(SetIsSystemUnderTestOption);

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        DisableLogging(builder);
        builder.ConfigureAppConfiguration(
            (_, configBuilder) =>
            {
                SetDefaultOptions(configBuilder);
                SetDatabaseConnectionString(configBuilder);
            }
        );
    }

    private void SetIsSystemUnderTestOption(IConfigurationBuilder configBuilder)
    {
        configBuilder.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                [EnvHandler.ConfigKey] = EnvHandler.ConfigValue
            }
        );
    }

    private void DisableLogging(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(
            x => x.ClearProviders()
        );
    }

    private void SetDefaultOptions(IConfigurationBuilder configBuilder)
    {
        foreach (IOptionsData optionsData in _defaultOptionsData)
        {
            configBuilder.AddInMemoryCollection(optionsData.ConvertToInMemoryCollection());
        }
    }

    private void SetDatabaseConnectionString(IConfigurationBuilder configBuilder)
    {
        configBuilder.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                [$"{DbConnectionStringsOptions.Position}:{nameof(DbConnectionStringsOptions.AppPostgresDb)}"] =
                    _dbContainer.GetConnectionString(),
                [$"{MongoDbConnectionStringsOptions.Position}:{nameof(MongoDbConnectionStringsOptions.MongoDb)}"] =
                    _mongoDbContainer.GetConnectionString() + MongoDbName
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
