using CommandDotNet;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.DataGeneratorCli;
using RunMethodsSequentially;

namespace R.Systems.Template.Tests.Api.DataGeneratorCli.Integration.Common;

public class ConsoleAppRunnerFactory : AppRunnerFactory, IAsyncLifetime
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

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    public override async Task<AppRunner> CreateAsync()
    {
        AddConfigurationMethods.Add(SetDatabaseConnectionString);
        ConfigureServicesMethods.Add(InitializeDatabase);

        return await base.CreateAsync();
    }

    private void SetDatabaseConnectionString(IConfigurationBuilder configBuilder)
    {
        configBuilder.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["ConnectionStrings:AppDb"] = BuildConnectionString()
            }
        );
    }

    private static void InitializeDatabase(IServiceCollection services)
    {
        services.RegisterRunMethodsSequentially(
                options =>
                {
                    options.RegisterAsHostedService = false;
                    options.AddFileSystemLockAndRunMethods(Environment.CurrentDirectory);
                }
            )
            .RegisterServiceToRunInJob<ConsoleSampleDataDbInitializer>();
    }

    private string BuildConnectionString()
    {
        return _dbContainer.ConnectionString + ";Trust Server Certificate=true";
    }
}
