using R.Systems.Template.Infrastructure.Db.SqlServer.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.ConnectionStrings;

internal class ConnectionStringsOptionsData : ConnectionStringsOptions, IOptionsData
{
    public ConnectionStringsOptionsData()
    {
        AppDb =
            "Server=127.0.0.1;Port=4044;Database=r-systems-template;User Id=r-systems-template;Password=rgre@#$2rewfgrRR;";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(AppDb)}"] = AppDb
        };
    }
}
