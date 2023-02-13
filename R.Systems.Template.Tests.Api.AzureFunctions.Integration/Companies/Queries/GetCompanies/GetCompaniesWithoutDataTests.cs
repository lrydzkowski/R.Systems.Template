using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Function;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.TestsCollections;
using FluentAssertions;
using R.Systems.Template.Api.AzureFunctions.Models;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Companies.Queries.GetCompanies;

[Collection(QueryWithoutDataTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryWithoutDataTestsCollection.CollectionName)]
public class GetCompaniesWithoutDataTests
{
    public GetCompaniesWithoutDataTests(FunctionFactory<NoDataDbInitializer> functionFactory)
    {
        Mapper = functionFactory.Services!.GetRequiredService<IMapper>();
        Mediator = functionFactory.Services!.GetRequiredService<ISender>();
    }

    private IMapper Mapper { get; }
    private ISender Mediator { get; }

    [Fact]
    public async Task GetCompanies_ShouldReturnEmptyList_WhenCompaniesNotExist()
    {
        GetCompaniesResult expectedResult = new()
        {
            Companies = new ListInfo<Company>()
        };

        ListParameters listParameters = Mapper.Map<ListParameters>(
            new ListRequest
            {
                Page = 1,
                PageSize = 100,
                SortingFieldName = null,
                SortingOrder = "",
                SearchQuery = null
            }
        );
        GetCompaniesResult result = await Mediator.Send(new GetCompaniesQuery { ListParameters = listParameters });

        result.Should().BeEquivalentTo(expectedResult);
    }
}
