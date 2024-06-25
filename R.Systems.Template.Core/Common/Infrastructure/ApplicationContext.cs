namespace R.Systems.Template.Core.Common.Infrastructure;

public class ApplicationContext
{
    public ApplicationContext(string? version)
    {
        Version = version ?? Versions.V1;
    }

    public ApplicationContext()
    {
        Version = Versions.V1;
    }

    public string Version { get; }
}
