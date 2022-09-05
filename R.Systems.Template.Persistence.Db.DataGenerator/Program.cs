using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace R.Systems.Template.Persistence.Db.DataGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            IConfigurationRoot configuration = BuildConfiguration();
            ServiceProvider serviceProvider = BuildServiceProvider(configuration);

            AppRunner appRunner = new(
                rootCommandType: typeof(CommandsHandler)
            );
            appRunner.UseDefaultMiddleware().UseMicrosoftDependencyInjection(serviceProvider).Run(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error has occurred!");
            Console.WriteLine(ex);
        }
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        string environmentName = Environment.GetEnvironmentVariable("APP_ENVIRONMENT") ?? "Production";
        IConfigurationBuilder confBuilder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();
        if (environmentName == "Development")
        {
            confBuilder = confBuilder.AddUserSecrets<Program>();
        }

        return confBuilder.Build();
    }

    private static ServiceProvider BuildServiceProvider(IConfigurationRoot configuration)
    {
        IServiceCollection services = new ServiceCollection();
        services.ConfigureServices();
        services.AddPersistenceDbService(configuration);

        return services.BuildServiceProvider();
    }
}
