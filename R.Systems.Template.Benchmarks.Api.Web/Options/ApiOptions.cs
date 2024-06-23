namespace R.Systems.Template.Benchmarks.Api.Web.Options;

internal static class ApiOptions
{
    public static string ApiBaseUrl => Environment.GetEnvironmentVariable("API_BASE_URL") ?? "";
    public static string AccessTokenAzureAd => Environment.GetEnvironmentVariable("ACCESS_TOKEN_AZURE_AD") ?? "";
    public static string AccessTokenAzureAdB2C => Environment.GetEnvironmentVariable("ACCESS_TOKEN_AZURE_AD_B2C") ?? "";
}
