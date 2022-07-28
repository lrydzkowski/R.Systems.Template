using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace R.Systems.Template.Persistence.Db;

public static class Initializer
{
    public static void DeployPersistenceDbMigration(this IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }
}
