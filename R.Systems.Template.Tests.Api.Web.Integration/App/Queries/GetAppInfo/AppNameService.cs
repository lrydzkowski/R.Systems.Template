using R.Systems.Template.Api.Web;

namespace R.Systems.Template.Tests.Api.Web.Integration.App.Queries.GetAppInfo;

internal static class AppNameService
{
    public static string GetWebApiName()
    {
        return typeof(Program).Namespace ?? "";
    }
}
