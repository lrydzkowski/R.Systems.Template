using MediatR;
using System.Reflection;

namespace R.Systems.Template.Core.App.Queries.GetAppInfo;

public class GetAppInfoQuery : IRequest<GetAppInfoResult>
{
    public Assembly AppAssembly { get; init; } = Assembly.GetExecutingAssembly();
}

public record GetAppInfoResult(string AppName, string AppVersion);

public class GetAppInfoHandler : IRequestHandler<GetAppInfoQuery, GetAppInfoResult>
{
    public Task<GetAppInfoResult> Handle(GetAppInfoQuery request, CancellationToken cancellationToken)
    {
        GetAppInfoResult result = new(
            AppVersion: GetAppVersion(request.AppAssembly),
            AppName: GetAppName(request.AppAssembly)
        );
        return Task.FromResult(result);
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
