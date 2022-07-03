using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace R.Systems.Template.Core;

public static class DependencyInjection
{
    public static void ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}
