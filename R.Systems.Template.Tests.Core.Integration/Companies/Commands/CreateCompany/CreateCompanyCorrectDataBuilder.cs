using Bogus;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Commands.CreateCompany;

internal static class CreateCompanyCorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();

        return new List<object[]>
        {
            BuildParameters(
                1,
                new CreateCompanyCommand
                {
                    Name = faker.Random.String2(100)
                }
            )
        };
    }

    private static object[] BuildParameters(
        int id,
        CreateCompanyCommand data
    )
    {
        return new object[] { id, data };
    }
}
