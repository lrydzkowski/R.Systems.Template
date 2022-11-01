using R.Systems.Template.Persistence.Db.Common.Options;
using R.Systems.Template.Tests.Integration.Common.Options;

namespace R.Systems.Template.Tests.Integration.Options.ConnectionStrings;

internal class ConnectionStringsOptionsData : IOptionsData
{
    public string? AppDb { get; set; }

    public ConnectionStringsOptionsData()
    {
        AppDb =
            "Server=127.0.0.1;Port=4044;Database=r-systems-template;User Id=r-systems-template;Password=rgre@#$2rewfgrRR;";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new()
        {
            [$"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppDb)}"] = AppDb
        };
    }
}
