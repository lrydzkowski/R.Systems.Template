using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace R.Systems.Template.WebApi.Swagger;

public static class ApplicationBuilderExtensions
{
    public static void UseSwagger(this IApplicationBuilder app)
    {
        IServiceProvider services = app.ApplicationServices;
        var provider = services.GetRequiredService<IApiVersionDescriptionProvider>();

        SwaggerBuilderExtensions.UseSwagger(app);

        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant()
                );
            }
        });
    }
}
