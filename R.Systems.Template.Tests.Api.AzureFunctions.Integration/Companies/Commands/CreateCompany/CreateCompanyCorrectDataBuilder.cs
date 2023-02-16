using Bogus;
using R.Systems.Template.Api.AzureFunctions.Models;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Companies.Commands.CreateCompany;

internal static class CreateCompanyCorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();

        return new List<object[]>
        {
            BuildParameters(
                1,
                new CreateCompanyRequest
                {
                    Name = faker.Random.String2(100)
                }
            )
        };
    }

    private static object[] BuildParameters(
        int id,
        CreateCompanyRequest data
    )
    {
        return new object[] { id, data };
    }
}
