using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db;
using RunMethodsSequentially;

namespace R.Systems.Template.Tests.Integration.Common.Db;

public class NoDataDbInitializer : IStartupServiceToRunSequentially
{
    public int OrderNum => 2;

    public async ValueTask ApplyYourChangeAsync(IServiceProvider scopedServices)
    {
        AppDbContext dbContext = scopedServices.GetRequiredService<AppDbContext>();
        await RemoveExistingDataAsync(dbContext);
    }

    private async Task RemoveExistingDataAsync(AppDbContext dbContext)
    {
        dbContext.RemoveRange(dbContext.Employees);
        dbContext.RemoveRange(dbContext.Companies);
        await dbContext.SaveChangesAsync();
    }
}
