using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db;
using R.Systems.Template.Tests.Integration.Common.Db;
using R.Systems.Template.WebApi;

namespace R.Systems.Template.Tests.Integration.Common.Factories;

public class WebApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(ConfigureServices);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        ReplaceDbContext(services);
    }

    private void ReplaceDbContext(IServiceCollection services)
    {
        RemoveService(services, typeof(DbContextOptions<AppDbContext>));
        string inMemoryDatabaseName = Guid.NewGuid().ToString();
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(inMemoryDatabaseName));

        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DbInitializer.InitializeData(dbContext);
    }

    private void RemoveService(IServiceCollection services, Type serviceType)
    {
        ServiceDescriptor? serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }
    }
}
