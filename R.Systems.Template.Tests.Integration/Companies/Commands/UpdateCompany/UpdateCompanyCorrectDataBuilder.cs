using Bogus;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.WebApi.Api;

namespace R.Systems.Template.Tests.Integration.Companies.Commands.UpdateCompany;

internal static class UpdateEmployeeCorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();

        return new List<object[]>
        {
            BuildParameters(
                IdGenerator.GetEmployeeId(1),
                IdGenerator.GetCompanyId(1),
                new UpdateCompanyRequest
                {
                    Name = faker.Random.String2(100)
                }
            )
        };
    }

    private static object[] BuildParameters(
        int id,
        int companyId,
        UpdateCompanyRequest data
    )
    {
        return new object[] { id, companyId, data };
    }
}
