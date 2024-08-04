using R.Systems.Template.Infrastructure.Azure.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.AzureStorageAccount;

internal class AzureStorageAccountOptionsData : AzureStorageAccountOptions, IOptionsData
{
    public AzureStorageAccountOptionsData()
    {
        Name = "test-name";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(Name)}"] = Name
        };
    }
}
