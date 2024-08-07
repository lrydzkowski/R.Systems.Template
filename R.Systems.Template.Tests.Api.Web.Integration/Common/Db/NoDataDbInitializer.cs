using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Infrastructure.PostgreSqlDb;
using RunMethodsSequentially;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Db;

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
