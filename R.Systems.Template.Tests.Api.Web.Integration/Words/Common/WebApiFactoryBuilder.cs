using Microsoft.AspNetCore.Mvc.Testing;
using R.Systems.Template.Api.Web;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;

namespace R.Systems.Template.Tests.Api.Web.Integration.Words.Common;

internal static class WebApiFactoryBuilder
{
    public static WebApplicationFactory<Program> WithWordnikApiBaseUrl(
        this WebApplicationFactory<Program> webApplicationFactory,
        string? apiBaseUrl
    )
    {
        return webApplicationFactory.WithCustomOptions(
            new Dictionary<string, string?>
            {
                ["Wordnik:ApiBaseUrl"] = apiBaseUrl
            }
        );
    }
}
