using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Infrastructure.Db;
using R.Systems.Template.Infrastructure.Db.SqlServer;
using RunMethodsSequentially;

namespace R.Systems.Template.Tests.Api.DataGeneratorCli.Integration.Common;

internal class ConsoleSampleDataDbInitializer : IStartupServiceToRunSequentially
{
    public int OrderNum => 1;

    public async ValueTask ApplyYourChangeAsync(IServiceProvider scopedServices)
    {
        AppDbContext dbContext = scopedServices.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        await RemoveExistingDataAsync(dbContext);
    }

    private async Task RemoveExistingDataAsync(AppDbContext dbContext)
    {
        dbContext.RemoveRange(dbContext.Employees);
        dbContext.RemoveRange(dbContext.Companies);
        await dbContext.SaveChangesAsync();
    }
}
