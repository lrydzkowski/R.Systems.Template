using CommandDotNet;
using CommandDotNet.TestTools;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.DataGeneratorCli;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.ConsoleAppRunner;

internal static class ConsoleAppRunnerFactoryBuilder
{
    public static AppRunnerFactory WithTestConsole(this AppRunnerFactory appRunnerFactory)
    {
        return appRunnerFactory.WithConfigureServices(
            services => services.AddScoped<IConsole, TestConsole>()
        );
    }

    public static AppRunnerFactory WithTestConsole(this AppRunnerFactory appRunnerFactory, ITestConsole testConsole)
    {
        return appRunnerFactory.WithConfigureServices(
            services => services.AddScoped<IConsole>(_ => testConsole)
        );
    }
}
