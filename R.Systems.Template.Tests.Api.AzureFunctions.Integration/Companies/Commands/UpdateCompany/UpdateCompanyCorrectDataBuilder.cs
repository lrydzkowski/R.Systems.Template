using Bogus;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Companies.Commands.UpdateCompany;

internal static class UpdateEmployeeCorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();

        return new List<object[]>
        {
            BuildParameters(
                1,
                (int)CompaniesSampleData.Data["Meta"].Id!,
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
