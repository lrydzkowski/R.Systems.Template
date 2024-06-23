using R.Systems.Template.Api.Web.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.Serilog;

internal class SerilogOptionsData : SerilogOptions, IOptionsData
{
    public SerilogOptionsData()
    {
        StorageAccount = new StorageAccountOptions
        {
            ConnectionString =
                "DefaultEndpointsProtocol=https;AccountName=accountname;AccountKey=1;EndpointSuffix=core.windows.net",
            ContainerName = "container-name"
        };
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(StorageAccount)}:{nameof(StorageAccount.ConnectionString)}"] =
                StorageAccount.ConnectionString,
            [$"{Position}:{nameof(StorageAccount)}:{nameof(StorageAccount.ContainerName)}"] =
                StorageAccount.ContainerName
        };
    }
}
