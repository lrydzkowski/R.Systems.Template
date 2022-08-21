using R.Systems.Template.Persistence.Db;
using R.Systems.Template.Persistence.Db.Common.Entities;

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
        foreach (KeyValuePair<string, CompanyEntity> element in CompaniesSampleData.Data)
        {
            dbContext.Companies.Add(element.Value);
        }
    }

    private static void AddTestEmployees(AppDbContext dbContext)
    {
        foreach (EmployeeEntity employeeEntity in EmployeesSampleData.Data)
        {
            dbContext.Employees.Add(employeeEntity);
        }
    }
}
