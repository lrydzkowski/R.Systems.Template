using R.Systems.Template.Infrastructure.Db.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.ConnectionStrings;

internal class ConnectionStringsOptionsData : ConnectionStringsOptions, IOptionsData
{
    public ConnectionStringsOptionsData()
    {
        AppPostgresDb =
            "Server=127.0.0.1;Database=r_systems_template;Port=5502;User Id=r_systems_template_user;Password=123";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(AppPostgresDb)}"] = AppPostgresDb
        };
    }
}
