using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using R.Systems.Template.Api.AzureFunctions.Middleware;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.PostgreSqlDb;

namespace R.Systems.Template.Api.AzureFunctions;

public class FunctionHostBuilder
{
    private readonly IHostBuilder _hostBuilder;

    public FunctionHostBuilder()
    {
        _hostBuilder = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults(builder => builder.UseMiddleware<ExceptionHandlingMiddleware>())
            .ConfigureAppConfiguration(
                builder =>
                {
                    builder.AddJsonFile("appsettings.json").AddUserSecrets<Program>().AddEnvironmentVariables();
                }
            )
            .ConfigureOpenApi()
            .ConfigureServices(
                (builder, services) =>
                {
                    services.ConfigureServices();
                    services.ConfigureCoreServices(builder.Configuration);
                    services.ConfigureInfrastructurePostgreSqlDbServices(builder.Configuration);
                }
            );
    }

    public IHost Build()
    {
        return _hostBuilder.Build();
    }

    public FunctionHostBuilder WithServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
    {
        _hostBuilder.ConfigureServices(configureDelegate);
        return this;
    }

    public FunctionHostBuilder WithAppConfiguration(Action<IConfigurationBuilder> configureDelegate)
    {
        _hostBuilder.ConfigureAppConfiguration(configureDelegate);
        return this;
    }
}
