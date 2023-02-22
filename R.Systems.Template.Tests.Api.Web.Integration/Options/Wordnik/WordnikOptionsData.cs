using R.Systems.Template.Infrastructure.Wordnik.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.Wordnik;

internal class WordnikOptionsData : WordnikOptions, IOptionsData
{
    public WordnikOptionsData()
    {
        ApiBaseUrl = "https://test.com";
        DefinitionsUrl = "/definitions";
        ApiKey = "rgerg34tg34gbrrfb";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(ApiBaseUrl)}"] = ApiBaseUrl,
            [$"{Position}:{nameof(DefinitionsUrl)}"] = DefinitionsUrl,
            [$"{Position}:{nameof(ApiKey)}"] = ApiKey
        };
    }
}
