using R.Systems.Template.Api.Web.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.Swagger;

internal class SwaggerOptionsData : SwaggerOptions, IOptionsData
{
    public SwaggerOptionsData()
    {
        Username = "username-test";
        Password = "password-test";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(Username)}"] = Username,
            [$"{Position}:{nameof(Password)}"] = Password
        };
    }
}
