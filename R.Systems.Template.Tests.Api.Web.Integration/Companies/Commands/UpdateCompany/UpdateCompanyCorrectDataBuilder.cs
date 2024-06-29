using Bogus;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

namespace R.Systems.Template.Tests.Api.Web.Integration.Companies.Commands.UpdateCompany;

internal static class UpdateEmployeeCorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();
        return new List<object[]>
        {
            BuildParameters(
                1,
                IdGenerator.GetCompanyId(1),
                new UpdateCompanyRequest { Name = faker.Random.String2(100) }
            )
        };
    }

    private static object[] BuildParameters(int id, long companyId, UpdateCompanyRequest data)
    {
        return new object[]
        {
            id,
            companyId,
            data
        };
    }
}
