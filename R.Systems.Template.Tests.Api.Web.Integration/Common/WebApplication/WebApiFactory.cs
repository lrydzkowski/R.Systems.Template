﻿using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.Web;
using R.Systems.Template.Infrastructure.Db.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAd;
using R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAdB2C;
using R.Systems.Template.Tests.Api.Web.Integration.Options.ConnectionStrings;
using R.Systems.Template.Tests.Api.Web.Integration.Options.Wordnik;
using RunMethodsSequentially;
using WireMock.Server;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;

public class WebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlTestcontainer _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
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

    private readonly List<IOptionsData> _defaultOptionsData = new()
    {
        new AzureAdOptionsData(), new AzureAdB2COptionsData(), new ConnectionStringsOptionsData(),
        new WordnikOptionsData()
    };

    public WireMockServer WireMockServer { get; }

    public WebApiFactory()
    {
        WireMockServer = WireMockServer.Start();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        WireMockServer.Dispose();
        await _dbContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(
            (_, configBuilder) =>
            {
                SetDefaultOptions(configBuilder);
                SetDatabaseConnectionString(configBuilder);
            }
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
                [$"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppDb)}"] =
                    BuildConnectionString()
            }
        );
    }

    private string BuildConnectionString()
    {
        return _dbContainer.ConnectionString + ";Trust Server Certificate=true";
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
