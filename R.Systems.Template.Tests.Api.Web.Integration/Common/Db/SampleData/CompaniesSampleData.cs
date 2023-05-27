using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Infrastructure.Db.Common.Entities;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

internal static class CompaniesSampleData
{
    public static Dictionary<string, CompanyEntity> Data { get; } = new()
    {
        {
            "Meta",
            new CompanyEntity
            {
                Id = IdGenerator.GetCompanyId(1),
                Name = "Meta"
            }
        },
        {
            "Google",
            new CompanyEntity
            {
                Id = IdGenerator.GetCompanyId(2),
                Name = "Google"
            }
        },
        {
            "Microsoft",
            new CompanyEntity
            {
                Id = IdGenerator.GetCompanyId(3),
                Name = "Microsoft"
            }
        },
        {
            "Amazon",
            new CompanyEntity
            {
                Id = IdGenerator.GetCompanyId(4),
                Name = "Amazon"
            }
        },
        {
            "Alior",
            new CompanyEntity
            {
                Id = IdGenerator.GetCompanyId(5),
                Name = "Alior"
            }
        },
        {
            "Starbucks",
            new CompanyEntity
            {
                Id = IdGenerator.GetCompanyId(6),
                Name = "Starbucks"
            }
        }
    };

    public static CompanyEntity Clone(this CompanyEntity companyEntity)
    {
        return new CompanyEntity
        {
            Name = companyEntity.Name
        };
    }

    public static List<Company> Companies
    {
        get
        {
            return Data
                .Where(x => x.Value.Id != null)
                .Select(
                    x => new Company
                    {
                        CompanyId = (int)x.Value.Id!,
                        Name = x.Value.Name
                    }
                )
                .ToList();
        }
    }
}
