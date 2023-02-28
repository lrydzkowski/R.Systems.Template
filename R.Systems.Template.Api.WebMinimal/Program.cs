using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.Infrastructure.Db.Postgres;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCoreServices();
builder.Services.ConfigureInfrastructureDbPostgresServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet(
    "/",
    async ([FromServices] ISender mediator) => Results.Ok(
        await mediator.Send(
            new GetAppInfoQuery { AppAssembly = Assembly.GetExecutingAssembly() }
        )
    )
);

app.Run();
