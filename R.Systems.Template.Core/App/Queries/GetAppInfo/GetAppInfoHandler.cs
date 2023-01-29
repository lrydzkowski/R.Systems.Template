using MediatR;
using System.Reflection;

namespace R.Systems.Template.Core.App.Queries.GetAppInfo;

public class GetAppInfoQuery : IRequest<GetAppInfoResult>
{
    public Assembly AppAssembly { get; init; } = Assembly.GetExecutingAssembly();
}

public class GetAppInfoResult
{
    public string AppName { get; init; } = "";

    public string AppVersion { get; init; } = "";
}

public class GetAppInfoHandler : RequestHandler<GetAppInfoQuery, GetAppInfoResult>
{
    protected override GetAppInfoResult Handle(GetAppInfoQuery request)
    {
        return new GetAppInfoResult
        {
            AppName = GetAppName(request.AppAssembly),
            AppVersion = GetAppVersion(request.AppAssembly)
        };
    }

    private string GetAppName(Assembly appAssembly)
    {
        return appAssembly.GetName().Name ?? "";
    }

    private string GetAppVersion(Assembly appAssembly)
    {
        return appAssembly
                   .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                   ?
                   .InformationalVersion
               ?? "";
    }
}
