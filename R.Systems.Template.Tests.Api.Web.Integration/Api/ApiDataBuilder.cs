using RestSharp;

namespace R.Systems.Template.Tests.Api.Web.Integration.Api;

internal class ApiDataBuilder
{
    public static IEnumerable<object?[]> Build()
    {
        return new List<object?[]>
        {
            new object?[]
            {
                "/companies/fafc71ef-5662-4c5f-b2c5-5dd3c8dfbbed",
                Method.Get
            },
            new object?[]
            {
                "/companies",
                Method.Get
            },
            new object?[]
            {
                "/companies",
                Method.Post
            },
            new object?[]
            {
                "/companies/fafc71ef-5662-4c5f-b2c5-5dd3c8dfbbed",
                Method.Put
            },
            new object?[]
            {
                "/employees/fafc71ef-5662-4c5f-b2c5-5dd3c8dfbbed",
                Method.Get
            },
            new object?[]
            {
                "/employees",
                Method.Get
            },
            new object?[]
            {
                "/employees",
                Method.Post
            },
            new object?[]
            {
                "/employees/fafc71ef-5662-4c5f-b2c5-5dd3c8dfbbed",
                Method.Put
            },
            new object?[]
            {
                "/companies/fafc71ef-5662-4c5f-b2c5-5dd3c8dfbbed/employees/fbc038e3-3cfe-4d87-a0e6-d18d6a5d2c0e",
                Method.Get
            },
            new object?[]
            {
                "/companies/fafc71ef-5662-4c5f-b2c5-5dd3c8dfbbed/employees",
                Method.Get
            },
            new object?[]
            {
                "/words/test/definitions",
                Method.Get
            }
        };
    }
}
