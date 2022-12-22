﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Persistence.Db;
using RunMethodsSequentially;

namespace R.Systems.Template.Tests.Integration.Common.ConsoleAppRunner;

internal class ConsoleSampleDataDbInitializer : IStartupServiceToRunSequentially
{
    public int OrderNum => 1;

    public async ValueTask ApplyYourChangeAsync(IServiceProvider scopedServices)
    {
        AppDbContext dbContext = scopedServices.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
