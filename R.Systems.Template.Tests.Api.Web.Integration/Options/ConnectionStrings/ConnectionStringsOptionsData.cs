using R.Systems.Template.Infrastructure.Db.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.ConnectionStrings;

internal class ConnectionStringsOptionsData : ConnectionStringsOptions, IOptionsData
{
    public ConnectionStringsOptionsData()
    {
        AppSqlServerDb =
            "Server=127.0.0.1;Port=4044;Database=r-systems-template;User Id=r-systems-template;Password=123";
        AppPostgresDb =
            "Server=127.0.0.1;Database=r_systems_template;Port=5502;User Id=r_systems_template_user;Password=123";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(AppSqlServerDb)}"] = AppSqlServerDb,
            [$"{Position}:{nameof(AppPostgresDb)}"] = AppPostgresDb
        };
    }
}
