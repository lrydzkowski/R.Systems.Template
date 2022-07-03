using Microsoft.AspNetCore.Mvc.Testing;

namespace R.Systems.Template.FunctionalTests;

public class WebApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
}
