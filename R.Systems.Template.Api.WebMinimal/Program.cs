using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.Infrastructure.Db;
using System.Reflection;
using R.Systems.Template.Infrastructure.Wordnik;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCoreServices(builder.Configuration);
builder.Services.ConfigureInfrastructureDbServices(builder.Configuration);
builder.Services.ConfigureInfrastructureWordnikServices(builder.Configuration);

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
