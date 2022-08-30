using Bogus;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;

namespace R.Systems.Template.Tests.Integration.Employees.Commands.CreateEmployee;

internal static class CreateEmployeeCorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();

        return new List<object[]>
        {
            BuildParameters(
                1,
                new CreateEmployeeCommand
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    CompanyId = CompaniesSampleData.Data["Meta"].Id
                }
            )
        };
    }

    private static object[] BuildParameters(
        int id,
        CreateEmployeeCommand data
    )
    {
        return new object[] { id, data };
    }
}
