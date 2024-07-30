using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Api.DataGeneratorCli.Services;

public interface IVersionedServiceFactory<out TService> where TService : IVersionedService
{
    TService GetService(string version);
}

internal class VersionedServiceFactory<TService> : IVersionedServiceFactory<TService>
    where TService : IVersionedService
{
    private readonly IEnumerable<TService> _services;

    public VersionedServiceFactory(IEnumerable<TService> services)
    {
        _services = services;
    }

    public TService GetService(string version)
    {
        if (!Versions.IsVersionAllowed(version))
        {
            version = Versions.V1;
        }

        return _services.First(x => x.Version.Equals(version, StringComparison.InvariantCultureIgnoreCase));
    }
}
