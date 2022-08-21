namespace R.Systems.Template.Tests.Integration.App.Queries.GetAppInfo;

internal static class AppNameService
{
    public static string GetWebApiName()
    {
        string testsProjectName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name ?? "";
        string webApiName = string.Join('.', testsProjectName.Split('.').Reverse().Skip(2).Reverse()) + ".WebApi";

        return webApiName;
    }
}
