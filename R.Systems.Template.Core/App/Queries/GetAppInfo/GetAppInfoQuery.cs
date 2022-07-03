using MediatR;
using System.Reflection;

namespace R.Systems.Template.Core.App.Queries.GetAppInfo;

public class GetAppInfoQuery : IRequest<AppInfo>
{
    public Assembly AppAssembly { get; init; } = Assembly.GetExecutingAssembly();
}

public class GetAppInfoHandler : IRequestHandler<GetAppInfoQuery, AppInfo>
{
    public Task<AppInfo> Handle(GetAppInfoQuery request, CancellationToken cancellationToken)
    {
        AppInfo app = new(
            AppVersion: GetAppVersion(request.AppAssembly),
            AppName: GetAppName(request.AppAssembly)
        );
        return Task.FromResult(app);
    }

    private string GetAppVersion(Assembly appAssembly)
    {
        return appAssembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "";
    }

    private string GetAppName(Assembly appAssembly)
    {
        return appAssembly.GetName().Name ?? "";
    }
}
