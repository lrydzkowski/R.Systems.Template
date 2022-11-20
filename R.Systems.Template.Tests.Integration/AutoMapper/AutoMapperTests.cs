using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Tests.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Integration.Common.WebApplication;

namespace R.Systems.Template.Tests.Integration.AutoMapper;

[Collection(QueryTestsCollection.CollectionName)]
public class AutoMapperTests
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
