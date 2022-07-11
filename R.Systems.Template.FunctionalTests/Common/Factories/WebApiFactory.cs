using Microsoft.AspNetCore.Mvc.Testing;

namespace R.Systems.Template.FunctionalTests.Common.Factories;

public class WebApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
}
