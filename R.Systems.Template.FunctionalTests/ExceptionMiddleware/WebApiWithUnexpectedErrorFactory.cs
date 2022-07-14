using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.App.Queries.GetAppInfo;

namespace R.Systems.Template.FunctionalTests.ExceptionMiddleware;

public class WebApiWithUnexpectedErrorFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(ReplaceGetAppInfoQuery);
    }

    private void ReplaceGetAppInfoQuery(IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<GetAppInfoQuery, GetAppInfoResult>, GetAppInfoHandlerWithException>();
    }
}
