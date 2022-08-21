using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db;
using R.Systems.Template.Tests.Integration.Common.Db;

namespace R.Systems.Template.Tests.Integration.Common.Factories;

public class WebApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ReplaceDbContext(services);
        });
    }

    private void ReplaceDbContext(IServiceCollection services)
    {
        RemoveService(services, typeof(DbContextOptions<AppDbContext>));
        string inMemoryDatabaseName = Guid.NewGuid().ToString();
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(inMemoryDatabaseName));

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();
        IServiceProvider scopedServiceProvider = scope.ServiceProvider;
        AppDbContext dbContext = GetService<AppDbContext>(scopedServiceProvider);

        dbContext.Database.EnsureCreated();
        DbInitializer.Initialize(dbContext);
    }

    private void RemoveService(IServiceCollection services, Type serviceType)
    {
        ServiceDescriptor? serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }
    }

    private T GetService<T>(IServiceProvider serviceProvider)
    {
        T? service = serviceProvider.GetService<T>();
        if (service == null)
        {
            throw new Exception($"Service {nameof(T)} doesn't exist.");
        }
        return service;
    }
}
