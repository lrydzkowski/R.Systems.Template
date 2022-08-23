using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Tests.Integration.Common.Db.SampleData;

internal static class CompaniesSampleData
{
    public static Dictionary<string, CompanyEntity> Data { get; } = new()
    {
        {
            "Meta",
            new CompanyEntity
            {
                Id = 1,
                Name = "Meta"
            }
        },
        {
            "Google",
            new CompanyEntity
            {
                Id = 2,
                Name = "Google"
            }
        },
        {
            "Microsoft",
            new CompanyEntity
            {
                Id = 3,
                Name = "Microsoft"
            }
        },
        {
            "Amazon",
            new CompanyEntity
            {
                Id = 4,
                Name = "Amazon"
            }
        }
    };
}
