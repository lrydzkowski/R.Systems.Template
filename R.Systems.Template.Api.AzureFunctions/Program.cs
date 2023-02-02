using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using R.Systems.Template.Api.AzureFunctions.Middleware;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Db;

namespace R.Systems.Template.Api.AzureFunctions;

public class Program
{
    public static void Main()
    {
        IHost? host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults(
                builder => builder.UseMiddleware<ExceptionHandlingMiddleware>()
            )
            .ConfigureAppConfiguration(
                builder =>
                {
                    builder.AddJsonFile("appsettings.json")
                        .AddUserSecrets<Program>()
                        .AddEnvironmentVariables();
                }
            )
            .ConfigureOpenApi()
            .ConfigureServices(
                (builder, services) =>
                {
                    services.ConfigureServices();
                    services.ConfigureCoreServices();
                    services.ConfigureInfrastructureDbServices(builder.Configuration);
                }
            )
            .Build();

        host.Run();
    }
}
