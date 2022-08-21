using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Tests.Integration.Common.SampleData;

internal static class EmployeesSampleData
{
    public static List<EmployeeEntity> Data { get; } = new()
    {
        new EmployeeEntity
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            CompanyId = 1
        },
        new EmployeeEntity
        {
            Id = 2,
            FirstName = "Jack",
            LastName = "Parker",
            CompanyId = 2
        },
        new EmployeeEntity
        {
            Id = 3,
            FirstName = "Will",
            LastName = "Smith",
            CompanyId = 2
        }
    };
}
