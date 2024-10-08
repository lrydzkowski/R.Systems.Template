﻿using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using Quartz;
using R.Systems.Template.Api.Web.Auth;
using R.Systems.Template.Api.Web.Hubs;
using R.Systems.Template.Api.Web.JsonConverters;
using R.Systems.Template.Api.Web.Options;
using R.Systems.Template.Api.Web.Services;
using R.Systems.Template.Api.Web.Swagger;
using R.Systems.Template.Core;
using R.Systems.Template.Core.Common.Infrastructure;
using PostgreSqlDbInitializer = R.Systems.Template.Infrastructure.PostgreSqlDb.AppDbInitializer;
using SqlServerDbInitializer = R.Systems.Template.Infrastructure.SqlServerDb.AppDbInitializer;
using RunMethodsSequentially;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web;

public static class DependencyInjection
{
    public const string CorsPolicy = "CorsPolicy";

    public static void ConfigureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonExceptionConverter()));
        services.AddMvc()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();
        services.ConfigureHttpLogging(configuration);
        services.ConfigureSignalR();
        services.ConfigureSwagger();
        services.ConfigureCors(configuration);
        services.ConfigureSequentialServices(environment);
        services.ChangeApiControllerModelValidationResponse();
        services.ConfigureOptions(configuration);
        services.ConfigureAuth();
        services.ConfigureQuartz(configuration);
    }

    private static void ConfigureHttpLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpLogging(
            logging =>
            {
                string[] headersToAdd = configuration["HeadersToAddToLogs"]?.Split(";") ?? Array.Empty<string>();
                foreach (string header in headersToAdd)
                {
                    logging.RequestHeaders.Add(header);
                }
            }
        );
    }

    private static void ConfigureSignalR(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<IUserIdProvider, EmailBasedUserIdProvider>();
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "R.Systems.Template.Api.Web", Version = "1.0" });
                options.EnableAnnotations();
                options.ExampleFilters();
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    }
                );
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                );
                options.OperationFilter<SwaggerHeaderParameterAttributeFilter>();
            }
        );
        services.AddSwaggerExamplesFromAssemblyOf(typeof(DependencyInjection));
    }

    private static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(
            options =>
            {
                string[] allowedOrigins = configuration["AllowedOrigins"]?.Split(";")
                                          ?? Array.Empty<string>();
                options.AddPolicy(
                    CorsPolicy,
                    builder => builder.WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                );
            }
        );
    }

    private static void ConfigureSequentialServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.RegisterRunMethodsSequentially(
                options => options.AddFileSystemLockAndRunMethods(environment.ContentRootPath)
            )
            .RegisterServiceToRunInJob<PostgreSqlDbInitializer>()
            .RegisterServiceToRunInJob<SqlServerDbInitializer>();
    }

    private static void ChangeApiControllerModelValidationResponse(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(
            options => options.InvalidModelStateResponseFactory =
                InvalidModelStateService.InvalidModelStateResponseFactory
        );
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<HealthCheckOptions, HealthCheckOptionsValidator>(
            configuration,
            HealthCheckOptions.Position
        );
        services.ConfigureOptionsWithValidation<SwaggerOptions, SwaggerOptionsValidator>(
            configuration,
            SwaggerOptions.Position
        );
    }

    private static void ConfigureAuth(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationSchemeOptions.Name,
                null
            );
    }

    private static void ConfigureQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        if (EnvHandler.IsSystemUnderTest(configuration))
        {
            return;
        }

        services.AddQuartz(
            q =>
            {
                JobKey jobKey = new(nameof(SendNotificationsJob));
                q.AddJob<SendNotificationsJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(
                    opts => opts
                        .ForJob(jobKey)
                        .WithIdentity($"{nameof(SendNotificationsJob)}-trigger")
                        //This Cron interval can be described as "run every minute" (when second is zero)
                        .WithCronSchedule("0 * * ? * *")
                );
            }
        );
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
