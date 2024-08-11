namespace R.Systems.Template.Core.Common.Infrastructure;

public static class Versions
{
    public const string V1 = "v1";
    public const string V2 = "v2";
    public const string V3 = "v3";
    public const string V4 = "v4";

    public static readonly IReadOnlyList<string> AllowedVersions = [V1, V2, V3, V4];

    public static bool IsVersionAllowed(string version)
    {
        return AllowedVersions.Contains(version, StringComparer.InvariantCultureIgnoreCase);
    }
}
