using R.Systems.Template.Persistence.Db;

namespace R.Systems.Template.Tests.Integration.Common.Db;

internal static class DbInitializer
{
    public static void Initialize(AppDbContext dbContext)
    {
        RemoveExistingData(dbContext);
        AddTestData(dbContext);
    }

    private static void RemoveExistingData(AppDbContext dbContext)
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
        dbContext.Companies.AddRange(CompaniesSampleData.Data.Select(x => x.Value));
    }

    private static void AddTestEmployees(AppDbContext dbContext)
    {
        dbContext.Employees.AddRange(EmployeesSampleData.Data);
    }
}
