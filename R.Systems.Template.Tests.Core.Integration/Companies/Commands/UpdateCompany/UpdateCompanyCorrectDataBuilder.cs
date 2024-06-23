using Bogus;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Tests.Core.Integration.Common.Db;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Commands.UpdateCompany;

internal static class UpdateCompanyCorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();
        return new List<object[]>
        {
            BuildParameters(
                1,
                new UpdateCompanyCommand
                    { CompanyId = (int)CompaniesSampleData.Data["Meta"].Id!, Name = faker.Random.String2(100) }
            )
        };
    }

    private static object[] BuildParameters(int id, UpdateCompanyCommand data)
    {
        return new object[]
        {
            id,
            data
        };
    }
}
