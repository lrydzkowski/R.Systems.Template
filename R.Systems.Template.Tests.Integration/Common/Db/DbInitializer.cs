using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Persistence.Db;
using R.Systems.Template.Persistence.Db.Common.Entities;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;

namespace R.Systems.Template.Tests.Integration.Common.Db;

internal static class DbInitializer
{
    public static void InitializeData(AppDbContext dbContext)
    {
        dbContext.Database.Migrate();
        RemoveExistingData(dbContext);
        AddTestData(dbContext);
    }

    public static void RemoveExistingData(AppDbContext dbContext)
    {
        dbContext.RemoveRange(dbContext.Employees);
        dbContext.RemoveRange(dbContext.Companies);
        dbContext.SaveChanges();
    }

    private static void AddTestData(AppDbContext dbContext)
    {
        AddTestCompanies(dbContext);
        AddTestEmployees(dbContext);
        dbContext.SaveChanges();
    }

    private static void AddTestCompanies(AppDbContext dbContext)
    {
        List<CompanyEntity> companies = CompaniesSampleData.Data.Select(x => x.Value).ToList();
        foreach (CompanyEntity company in companies)
        {
            dbContext.Companies.Add(company.Clone());
        }
    }

    private static void AddTestEmployees(AppDbContext dbContext)
    {
        List<EmployeeEntity> employees = EmployeesSampleData.Data.ToList();
        foreach (EmployeeEntity employee in employees)
        {
            dbContext.Employees.Add(employee.Clone());
        }
    }
}
