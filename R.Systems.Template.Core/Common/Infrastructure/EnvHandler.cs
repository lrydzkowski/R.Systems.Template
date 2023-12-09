using Microsoft.Extensions.Configuration;

namespace R.Systems.Template.Core.Common.Infrastructure;

public static class EnvHandler
{
    public const string ConfigKey = "IsSystemUnderTest";

    public const string ConfigValue = "1";

    public static bool IsSystemUnderTest(IConfiguration configuration)
    {
        return configuration[ConfigKey] == ConfigValue;
    }
}
