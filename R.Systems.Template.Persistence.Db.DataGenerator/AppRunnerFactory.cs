using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace R.Systems.Template.Persistence.Db.DataGenerator;

public class AppRunnerFactory
{
    protected List<Action<IConfigurationBuilder>> AddConfigurationMethods { get; } = new();

    protected List<Action<IServiceCollection>> ConfigureServicesMethods { get; } = new();

    protected IServiceProvider? ServiceProvider { get; private set; }

    public AppRunnerFactory WithConfigureServices(Action<IServiceCollection> configureServices)
    {
        ConfigureServicesMethods.Add(configureServices);

        return this;
    }

    public virtual AppRunner Create()
    {
        IConfigurationRoot configuration = BuildConfiguration();
        ServiceProvider = BuildServiceProvider(configuration);

        AppRunner appRunner = new(rootCommandType: typeof(CommandsHandler));

        return appRunner.UseDefaultMiddleware().UseMicrosoftDependencyInjection(ServiceProvider);
    }

    private IConfigurationRoot BuildConfiguration()
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

        foreach (Action<IConfigurationBuilder> addConfigurationBuilder in AddConfigurationMethods)
        {
            addConfigurationBuilder(confBuilder);
        }

        return confBuilder.Build();
    }

    private IServiceProvider BuildServiceProvider(IConfigurationRoot configuration)
    {
        IServiceCollection services = new ServiceCollection();
        services.ConfigureServices();
        services.ConfigurePersistenceDbServices(configuration);
        foreach (Action<IServiceCollection> configureServicesMethod in ConfigureServicesMethods)
        {
            configureServicesMethod(services);
        }

        return services.BuildServiceProvider();
    }
}
