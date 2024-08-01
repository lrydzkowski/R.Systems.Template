using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;

namespace R.Systems.Template.Tests.Core.Integration.Common.Db;

internal static class CompaniesSampleData
{
    public static Dictionary<string, CompanyEntity> Data { get; } = new()
    {
        {
            "Meta",
            new CompanyEntity
            {
                Id = new Guid("01910F2B-6097-BE74-8EEC-5A3F0D44249A"),
                Name = "Meta"
            }
        },
        {
            "Google",
            new CompanyEntity
            {
                Id = new Guid("01910F2B-8D9C-FD65-F87B-8CF4812779A0"),
                Name = "Google"
            }
        },
        {
            "Microsoft",
            new CompanyEntity
            {
                Id = new Guid("01910F2B-9C9E-A9FB-9057-FEE6FA9EEE07"),
                Name = "Microsoft"
            }
        },
        {
            "Amazon",
            new CompanyEntity
            {
                Id = new Guid("01910F2B-ABCC-7D1C-65ED-7AEBE225742F"),
                Name = "Amazon"
            }
        },
        {
            "Alior",
            new CompanyEntity
            {
                Id = new Guid("01910F2B-BC3F-8E60-72D9-6B6AB71EF1DB"),
                Name = "Alior"
            }
        },
        {
            "Starbucks",
            new CompanyEntity
            {
                Id = new Guid("01910F2B-CD41-6842-ADDD-7D9357541D55"),
                Name = "Starbucks"
            }
        }
    };

    public static List<Company> Companies
    {
        get
        {
            return Data.Where(x => x.Value.Id != null)
                .Select(x => new Company { CompanyId = (Guid)x.Value.Id!, Name = x.Value.Name })
                .ToList();
        }
    }
}
