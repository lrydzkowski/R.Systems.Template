using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace R.Systems.Template.Infrastructure.SqlServerDb.Health;

internal class SqlServerDbHealthCheck : IHealthCheck
{
    private readonly AppDbContext _appDbContext;

    public SqlServerDbHealthCheck(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new()
    )
    {
        try
        {
            await _appDbContext.Companies.Select(company => company.Id)
                .OrderBy(id => id)
                .Take(10)
                .ToListAsync(cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
