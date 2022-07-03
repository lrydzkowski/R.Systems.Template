namespace R.Systems.Template.FunctionalTests.App.Queries.GetAppInfo;

internal static class AppNameService
{
    public static string GetWebApiName()
    {
        string testsProjectName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name ?? "";
        string webApiName = string.Join('.', testsProjectName.Split('.').Reverse().Skip(1).Reverse()) + ".WebApi";

        return webApiName;
    }
}
