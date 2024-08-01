using Bogus;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Commands.UpdateEmployee;

internal static class UpdateEmployeeCorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();
        return new List<object[]>
        {
            BuildParameters(
                1,
                (Guid)EmployeesSampleData.Data[0].Id!,
                new UpdateEmployeeRequest
                {
                    FirstName = faker.Name.FirstName(), LastName = faker.Name.LastName(),
                    CompanyId = CompaniesSampleData.Data["Meta"].Id.ToString()!
                }
            )
        };
    }

    private static object[] BuildParameters(int id, Guid employeeId, UpdateEmployeeRequest data)
    {
        return new object[]
        {
            id,
            employeeId,
            data
        };
    }
}
