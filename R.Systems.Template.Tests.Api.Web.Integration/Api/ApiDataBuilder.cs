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
                "/companies/1",
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
                "/companies/1",
                Method.Put
            },
            new object?[]
            {
                "/employees/1",
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
                "/employees/1",
                Method.Put
            },
            new object?[]
            {
                "/companies/1/employees/1",
                Method.Get
            },
            new object?[]
            {
                "/companies/1/employees",
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
