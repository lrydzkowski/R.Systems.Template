using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Tests.Integration.Common.Factories;

namespace R.Systems.Template.Tests.Integration.AutoMapper;

public class AutoMapperTests : IClassFixture<WebApiFactory>
{
    public AutoMapperTests(WebApiFactory webApiFactory)
    {
        WebApiFactory = webApiFactory;
    }

    private WebApiFactory WebApiFactory { get; }

    [Fact]
    public void AutoMapperConfiguration_ShouldBeValid()
    {
        IMapper? mapper = WebApiFactory.Services.GetService<IMapper>();
        mapper?.ConfigurationProvider.AssertConfigurationIsValid();

        Assert.NotNull(mapper);
    }
}
