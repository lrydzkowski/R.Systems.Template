namespace R.Systems.Template.Tests.Api.Web.Integration.App.Queries.GetAppInfo;

internal class SemVerRegex
{
    private readonly string _buildMetadata = @"(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?";
    private readonly string _major = @"(0|[1-9]\d*)";
    private readonly string _minor = @"\.(0|[1-9]\d*)";
    private readonly string _path = @"\.(0|[1-9]\d*)";

    private readonly string _preRelease =
        @"(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?";

    public string Get()
    {
        return $"^{_major}{_minor}{_path}{_preRelease}{_buildMetadata}$";
    }
}
