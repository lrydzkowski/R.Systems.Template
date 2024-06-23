using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Infrastructure.Db;
using RunMethodsSequentially.LockAndRunCode;

namespace R.Systems.Template.Api.DataGeneratorCli;

public class AppRunnerFactory
{
    private IServiceProvider? _serviceProvider;
    protected List<Action<IConfigurationBuilder>> AddConfigurationMethods { get; } = new();
    protected List<Action<IServiceCollection>> ConfigureServicesMethods { get; } = new();

    public AppRunnerFactory WithConfigureServices(Action<IServiceCollection> configureServices)
    {
        ConfigureServicesMethods.Add(configureServices);
        return this;
    }

    public virtual async Task<AppRunner> CreateAsync()
    {
        IConfigurationRoot configuration = BuildConfiguration();
        _serviceProvider = BuildServiceProvider(configuration);
        await RunStartupMethodsSequentially(_serviceProvider);
        AppRunner appRunner = new(typeof(CommandsHandler));
        return appRunner.UseDefaultMiddleware().UseMicrosoftDependencyInjection(_serviceProvider);
    }

    private IConfigurationRoot BuildConfiguration()
    {
        string environmentName = Environment.GetEnvironmentVariable("APP_ENVIRONMENT") ?? "Production";
        IConfigurationBuilder confBuilder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
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
        services.ConfigureInfrastructureDbServices(configuration);
        foreach (Action<IServiceCollection> configureServicesMethod in ConfigureServicesMethods)
        {
            configureServicesMethod(services);
        }

        return services.BuildServiceProvider();
    }

    private async Task RunStartupMethodsSequentially(IServiceProvider serviceProvider)
    {
        IGetLockAndThenRunServices? service = serviceProvider.GetService<IGetLockAndThenRunServices>();
        if (service != null)
        {
            await service.LockAndLoadAsync();
        }
    }
}
