using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Quartz;
using R.Systems.Template.Api.Web.Auth;
using R.Systems.Template.Api.Web.Options;
using R.Systems.Template.Api.Web.Services;
using R.Systems.Template.Core;
using R.Systems.Template.Infrastructure.Db;
using RunMethodsSequentially;

namespace R.Systems.Template.Api.Web;

public static class DependencyInjection
{
    public static void ConfigureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();
        services.ConfigureSwagger();
        services.ConfigureCors();
        services.ConfigureSequentialServices(environment);
        services.ChangeApiControllerModelValidationResponse();
        services.ConfigureOptions(configuration);
        services.ConfigureAuth();
        services.ConfigureQuartz();
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "R.Systems.Template.Api.Web", Version = "1.0" });
                options.EnableAnnotations();
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
            }
        );
    }

    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            }
        );
    }

    private static void ConfigureSequentialServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.RegisterRunMethodsSequentially(
                options => options.AddFileSystemLockAndRunMethods(environment.ContentRootPath)
            )
            .RegisterServiceToRunInJob<AppDbInitializer>();
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
    }

    private static void ConfigureAuth(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationSchemeOptions.Name,
                null
            );
    }

    private static void ConfigureQuartz(this IServiceCollection services)
    {
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
