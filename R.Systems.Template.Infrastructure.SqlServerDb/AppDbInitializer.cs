using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RunMethodsSequentially;

namespace R.Systems.Template.Infrastructure.SqlServerDb;

public class AppDbInitializer : IStartupServiceToRunSequentially
{
    public int OrderNum => 2;

    public async ValueTask ApplyYourChangeAsync(IServiceProvider scopedServices)
    {
        AppDbContext dbContext = scopedServices.GetRequiredService<AppDbContext>();
        if (dbContext.Database.IsRelational())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
