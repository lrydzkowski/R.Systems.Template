using R.Systems.Template.FunctionalTests.Common.SampleData;
using R.Systems.Template.Persistence.Db;

namespace R.Systems.Template.FunctionalTests.Common.Initializers;

internal static class AppDbInitializer
{
    public static void InitData(AppDbContext dbContext)
    {
        AddCompanies(dbContext);
        AddEmployees(dbContext);
        dbContext.SaveChanges();
    }

    private static void AddCompanies(AppDbContext dbContext)
    {
        dbContext.Companies.AddRange(CompaniesSampleData.Data.Select(x => x.Value));
    }

    private static void AddEmployees(AppDbContext dbContext)
    {
        dbContext.Employees.AddRange(EmployeesSampleData.Data);
    }
}
