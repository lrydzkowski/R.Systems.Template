﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;

namespace R.Systems.Template.Tests.Api.Web.Integration.AutoMapper;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class AutoMapperTests
{
    public AutoMapperTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
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