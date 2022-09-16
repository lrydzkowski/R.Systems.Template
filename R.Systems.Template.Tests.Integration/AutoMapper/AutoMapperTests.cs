using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;

namespace R.Systems.Template.Tests.Integration.AutoMapper;

public class AutoMapperTests
{
    [Fact]
    public void AutoMapperConfiguration_ShouldBeValid()
    {
        WebApiFactory<Program> webApiFactory = new();
        IMapper? mapper = webApiFactory.Services.GetService<IMapper>();
        mapper?.ConfigurationProvider.AssertConfigurationIsValid();

        Assert.NotNull(mapper);
    }
}
