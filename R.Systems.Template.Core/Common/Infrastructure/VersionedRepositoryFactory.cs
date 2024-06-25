namespace R.Systems.Template.Core.Common.Infrastructure;

public interface IVersionedRepositoryFactory<out TRepository> where TRepository : IVersionedRepository
{
    TRepository GetRepository(ApplicationContext appContext);
}

internal class VersionedRepositoryFactory<TRepository> : IVersionedRepositoryFactory<TRepository>
    where TRepository : IVersionedRepository
{
    private readonly IEnumerable<TRepository> _services;

    public VersionedRepositoryFactory(IEnumerable<TRepository> services)
    {
        _services = services;
    }

    public TRepository GetRepository(ApplicationContext appContext)
    {
        string version = appContext.Version;
        if (!Versions.IsVersionAllowed(version))
        {
            version = Versions.V1;
        }

        return _services.First(x => x.Version.Equals(version, StringComparison.InvariantCultureIgnoreCase));
    }
}
