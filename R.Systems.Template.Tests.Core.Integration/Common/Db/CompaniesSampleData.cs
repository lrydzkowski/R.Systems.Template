using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Infrastructure.Db.Common.Entities;

namespace R.Systems.Template.Tests.Core.Integration.Common.Db;

internal static class CompaniesSampleData
{
    public static Dictionary<string, CompanyEntity> Data { get; } = new()
    {
        {
            "Meta",
            new CompanyEntity
            {
                Id = 3,
                Name = "Meta"
            }
        },
        {
            "Google",
            new CompanyEntity
            {
                Id = 4,
                Name = "Google"
            }
        },
        {
            "Microsoft",
            new CompanyEntity
            {
                Id = 5,
                Name = "Microsoft"
            }
        },
        {
            "Amazon",
            new CompanyEntity
            {
                Id = 6,
                Name = "Amazon"
            }
        },
        {
            "Alior",
            new CompanyEntity
            {
                Id = 7,
                Name = "Alior"
            }
        },
        {
            "Starbucks",
            new CompanyEntity
            {
                Id = 8,
                Name = "Starbucks"
            }
        }
    };

    public static List<Company> Companies
    {
        get
        {
            return Data.Where(x => x.Value.Id != null)
                .Select(x => new Company { CompanyId = (int)x.Value.Id!, Name = x.Value.Name })
                .ToList();
        }
    }
}
