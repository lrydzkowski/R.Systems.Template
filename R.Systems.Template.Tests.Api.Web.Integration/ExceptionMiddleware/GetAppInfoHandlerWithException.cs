using MediatR;
using R.Systems.Template.Core.App.Queries.GetAppInfo;

namespace R.Systems.Template.Tests.Api.Web.Integration.ExceptionMiddleware;

public class GetAppInfoHandlerWithException : IRequestHandler<GetAppInfoQuery, GetAppInfoResult>
{
    public Task<GetAppInfoResult> Handle(GetAppInfoQuery request, CancellationToken cancellationToken)
    {
        throw new Exception("Test Exception.");
    }
}
