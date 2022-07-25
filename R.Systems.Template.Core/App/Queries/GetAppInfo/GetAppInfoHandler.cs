using MediatR;
using System.Reflection;

namespace R.Systems.Template.Core.App.Queries.GetAppInfo;

public class GetAppInfoQuery : IRequest<GetAppInfoResult>
{
    public Assembly AppAssembly { get; init; } = Assembly.GetExecutingAssembly();
}

public record GetAppInfoResult()
{
    public string AppName { get; init; } = "";

    public string AppVersion { get; init; } = "";
}

public class GetAppInfoHandler : IRequestHandler<GetAppInfoQuery, GetAppInfoResult>
{
    public Task<GetAppInfoResult> Handle(GetAppInfoQuery request, CancellationToken cancellationToken)
    {
        GetAppInfoResult result = new()
        {
            AppName = GetAppName(request.AppAssembly),
            AppVersion = GetAppVersion(request.AppAssembly)
        };

        return Task.FromResult(result);
    }

    private string GetAppName(Assembly appAssembly)
    {
        return appAssembly.GetName().Name ?? "";
    }

    private string GetAppVersion(Assembly appAssembly)
    {
        return appAssembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "";
    }
}
