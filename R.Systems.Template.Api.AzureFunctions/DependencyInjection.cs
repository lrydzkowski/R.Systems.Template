﻿using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.AzureFunctions.Services;

namespace R.Systems.Template.Api.AzureFunctions;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddSingleton<HttpResponseBuilder>();
        services.AddSingleton<CustomJsonSerializer>();
    }
}