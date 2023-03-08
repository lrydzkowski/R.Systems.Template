using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Infrastructure.Db;
using R.Systems.Template.Infrastructure.Db.Common.Entities;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;
using RunMethodsSequentially;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Db;

public class SampleDataDbInitializer : IStartupServiceToRunSequentially
{
    public int OrderNum => 3;

    public async ValueTask ApplyYourChangeAsync(IServiceProvider scopedServices)
    {
        AppDbContext dbContext = scopedServices.GetRequiredService<AppDbContext>();
        if (await DataExistsAsync(dbContext))
        {
            return;
        }

        await RemoveExistingDataAsync(dbContext);
        await AddTestDataAsync(dbContext);
    }

    private async Task<bool> DataExistsAsync(AppDbContext dbContext)
    {
        int? maxId = CompaniesSampleData.Data.Max(y => y.Value.Id);
        if (maxId == null)
        {
            return false;
        }

        CompanyEntity? companyEntity = await dbContext.Companies.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == maxId);

        return companyEntity != null;
    }

    private async Task RemoveExistingDataAsync(AppDbContext dbContext)
    {
        dbContext.RemoveRange(dbContext.Employees);
        dbContext.RemoveRange(dbContext.Companies);
        await dbContext.SaveChangesAsync();
    }

    private async Task AddTestDataAsync(AppDbContext dbContext)
    {
        await AddTestCompaniesAsync(dbContext);
        await AddTestEmployeesAsync(dbContext);
        await dbContext.SaveChangesAsync();
    }

    private async Task AddTestCompaniesAsync(AppDbContext dbContext)
    {
        List<CompanyEntity> companies = CompaniesSampleData.Data.Select(x => x.Value).ToList();
        foreach (CompanyEntity company in companies)
        {
            await dbContext.Companies.AddAsync(company.Clone());
        }
    }

    private async Task AddTestEmployeesAsync(AppDbContext dbContext)
    {
        List<EmployeeEntity> employees = EmployeesSampleData.Data.ToList();
        foreach (EmployeeEntity employee in employees)
        {
            await dbContext.Employees.AddAsync(employee.Clone());
        }
    }
}
