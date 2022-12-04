using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using R.Systems.Template.AzureFunctionsApi.Middleware;
using R.Systems.Template.Core;
using R.Systems.Template.Persistence.Db;

namespace R.Systems.Template.AzureFunctionsApi;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
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
                    services.ConfigurePersistenceDbServices(builder.Configuration);
                }
            )
            .Build();

        host.Run();
    }
}
