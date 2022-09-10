using CommandDotNet;
using CommandDotNet.TestTools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db;
using R.Systems.Template.Persistence.Db.DataGenerator;
using R.Systems.Template.Tests.Integration.Common.Db;

namespace R.Systems.Template.Tests.Integration.Common.Builders;

internal static class AppRunnerFactoryBuilder
{
    public static AppRunnerFactory WithDatabaseInMemory(this AppRunnerFactory appRunnerFactory)
    {
        return appRunnerFactory.WithConfigureServices(services => services.ReplaceDbContext());
    }

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

    private static void ReplaceDbContext(this IServiceCollection services)
    {
        services.RemoveService(typeof(DbContextOptions<AppDbContext>));
        string inMemoryDatabaseName = Guid.NewGuid().ToString();
        services.AddDbContext<AppDbContext>(
            options => options.UseInMemoryDatabase(inMemoryDatabaseName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        );

        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DbInitializer.InitializeData(dbContext);
    }

    private static void RemoveService(this IServiceCollection services, Type serviceType)
    {
        ServiceDescriptor? serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }
    }
}
