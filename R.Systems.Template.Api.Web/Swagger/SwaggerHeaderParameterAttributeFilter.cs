using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace R.Systems.Template.Api.Web.Swagger;

public class SwaggerHeaderParameterAttributeFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        List<SwaggerHeaderParameterAttribute> attributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                                                               .Union(context.MethodInfo.GetCustomAttributes(true))
                                                               .OfType<SwaggerHeaderParameterAttribute>()
                                                               .ToList()
                                                           ?? [];

        foreach (SwaggerHeaderParameterAttribute attribute in attributes)
        {
            operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = attribute.Name,
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema { Type = "string" }
                }
            );
        }
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class SwaggerHeaderParameterAttribute : Attribute
{
    public SwaggerHeaderParameterAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
