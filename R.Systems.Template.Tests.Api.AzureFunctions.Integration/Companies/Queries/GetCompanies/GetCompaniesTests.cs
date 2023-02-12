using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Function;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.TestsCollections;
using System.Linq.Dynamic.Core;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Companies.Queries.GetCompanies;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetCompaniesTests
{
    public GetCompaniesTests(FunctionFactory<SampleDataDbInitializer> functionFactory)
    {
        Mapper = functionFactory.Services!.GetRequiredService<IMapper>();
        Mediator = functionFactory.Services!.GetRequiredService<ISender>();
    }

    private IMapper Mapper { get; }
    private ISender Mediator { get; }

    [Fact]
    public async Task GetCompanies_ShouldReturnCompanies_WhenCompaniesExist()
    {
        GetCompaniesResult expectedResult = new()
        {
            Companies = new ListInfo<Company>
            {
                Data = CompaniesSampleData.Companies,
                Count = CompaniesSampleData.Companies.Count
            }
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

    [Theory]
    [InlineData(2, 4)]
    [InlineData(1, 10)]
    public async Task GetCompanies_ShouldReturnPaginatedCompanies_WhenPaginationParametersArePassed(
        int page,
        int pageSize
    )
    {
        IQueryable<Company> expectedCompanies = CompaniesSampleData.Companies.OrderBy(x => x.CompanyId).AsQueryable();
        GetCompaniesResult expectedResult = new()
        {
            Companies = new ListInfo<Company>
            {
                Data = expectedCompanies
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList(),
                Count = expectedCompanies.Count()
            }
        };

        ListParameters listParameters = Mapper.Map<ListParameters>(
            new ListRequest
            {
                Page = page,
                PageSize = pageSize,
                SortingFieldName = null,
                SortingOrder = "",
                SearchQuery = null
            }
        );
        GetCompaniesResult result = await Mediator.Send(new GetCompaniesQuery { ListParameters = listParameters });

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    public async Task GetCompanies_ShouldReturnSortedCompanies_WhenSortingParametersArePassed(
        string sortingFieldName,
        string sortingOrder
    )
    {
        IQueryable<Company> expectedCompanies =
            CompaniesSampleData.Companies.AsQueryable().OrderBy($"{sortingFieldName} {sortingOrder}");
        GetCompaniesResult expectedResult = new()
        {
            Companies = new ListInfo<Company>
            {
                Data = expectedCompanies.ToList(),
                Count = expectedCompanies.Count()
            }
        };

        ListParameters listParameters = Mapper.Map<ListParameters>(
            new ListRequest
            {
                Page = 1,
                PageSize = 100,
                SortingFieldName = sortingFieldName,
                SortingOrder = sortingOrder,
                SearchQuery = null
            }
        );
        GetCompaniesResult result = await Mediator.Send(new GetCompaniesQuery { ListParameters = listParameters });

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData("Microsoft")]
    [InlineData("microsofT")]
    [InlineData("croso")]
    [InlineData("o")]
    public async Task GetCompanies_ShouldReturnFilteredCompanies_WhenSearchParametersArePassed(string searchQuery)
    {
        IQueryable<Company> expectedCompanies = CompaniesSampleData.Companies
            .Where(x => x.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
            .AsQueryable();
        GetCompaniesResult expectedResult = new()
        {
            Companies = new ListInfo<Company>
            {
                Data = expectedCompanies.ToList(),
                Count = expectedCompanies.Count()
            }
        };

        ListParameters listParameters = Mapper.Map<ListParameters>(
            new ListRequest
            {
                Page = 1,
                PageSize = 100,
                SortingFieldName = null,
                SortingOrder = "",
                SearchQuery = searchQuery
            }
        );
        GetCompaniesResult result = await Mediator.Send(new GetCompaniesQuery { ListParameters = listParameters });

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetCompanies_ShouldReturnCorrectCompanies_WhenParametersArePassed()
    {
        int page = 2;
        int pageSize = 2;
        string sortingFieldName = "name";
        string sortingOrder = "asc";
        string searchQuery = "o";

        IQueryable<Company> expectedCompanies = CompaniesSampleData.Companies
            .OrderBy(x => x.Name)
            .Where(x => x.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
            .AsQueryable();
        GetCompaniesResult expectedResult = new()
        {
            Companies = new ListInfo<Company>
            {
                Data = expectedCompanies
                    // ReSharper disable once UselessBinaryOperation
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList(),
                Count = expectedCompanies.Count()
            }
        };

        ListParameters listParameters = Mapper.Map<ListParameters>(
            new ListRequest
            {
                Page = page,
                PageSize = pageSize,
                SortingFieldName = sortingFieldName,
                SortingOrder = sortingOrder,
                SearchQuery = searchQuery
            }
        );
        GetCompaniesResult result = await Mediator.Send(new GetCompaniesQuery { ListParameters = listParameters });

        result.Should().BeEquivalentTo(expectedResult);
    }
}
