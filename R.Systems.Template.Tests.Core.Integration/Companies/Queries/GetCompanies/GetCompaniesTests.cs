using System.Linq.Dynamic.Core;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Tests.Core.Integration.Common;
using R.Systems.Template.Tests.Core.Integration.Common.Db;
using R.Systems.Template.Tests.Core.Integration.Common.TestsCollections;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Queries.GetCompanies;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetCompaniesTests
{
    private readonly ISender _mediator;

    public GetCompaniesTests(SystemUnderTest<SampleDataDbInitializer> systemUnderTest)
    {
        _mediator = systemUnderTest.BuildServiceProvider().GetRequiredService<ISender>();
    }

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
        GetCompaniesQuery query = new()
        {
            ListParameters = new ListParameters
            {
                Pagination = new Pagination
                {
                    Page = 1,
                    PageSize = 100
                },
                Sorting = new Sorting
                {
                    FieldName = null,
                    Order = SortingOrder.Ascending
                },
                Search = new Search
                {
                    Query = null
                }
            }
        };
        GetCompaniesResult result = await _mediator.Send(query);
        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.Companies.Data));
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
                Data = expectedCompanies.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                Count = expectedCompanies.Count()
            }
        };
        GetCompaniesQuery query = new()
        {
            ListParameters = new ListParameters
            {
                Pagination = new Pagination
                {
                    Page = page,
                    PageSize = pageSize
                },
                Sorting = new Sorting
                {
                    FieldName = null,
                    Order = SortingOrder.Ascending
                },
                Search = new Search
                {
                    Query = null
                }
            }
        };
        GetCompaniesResult result = await _mediator.Send(query);
        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.Companies.Data));
    }

    [Theory]
    [InlineData("name", SortingOrder.Ascending)]
    [InlineData("name", SortingOrder.Descending)]
    public async Task GetCompanies_ShouldReturnSortedCompanies_WhenSortingParametersArePassed(
        string sortingFieldName,
        SortingOrder sortingOrder
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
        GetCompaniesQuery query = new()
        {
            ListParameters = new ListParameters
            {
                Pagination = new Pagination
                {
                    Page = 1,
                    PageSize = 100
                },
                Sorting = new Sorting
                {
                    FieldName = sortingFieldName,
                    Order = sortingOrder
                },
                Search = new Search
                {
                    Query = null
                }
            }
        };
        GetCompaniesResult result = await _mediator.Send(query);
        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.Companies.Data));
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
        GetCompaniesQuery query = new()
        {
            ListParameters = new ListParameters
            {
                Pagination = new Pagination
                {
                    Page = 1,
                    PageSize = 100
                },
                Sorting = new Sorting
                {
                    FieldName = null,
                    Order = SortingOrder.Ascending
                },
                Search = new Search
                {
                    Query = searchQuery
                }
            }
        };
        GetCompaniesResult result = await _mediator.Send(query);
        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.Companies.Data));
    }

    [Fact]
    public async Task GetCompanies_ShouldReturnCorrectCompanies_WhenParametersArePassed()
    {
        int page = 2;
        int pageSize = 2;
        string sortingFieldName = "name";
        SortingOrder sortingOrder = SortingOrder.Ascending;
        string searchQuery = "o";
        IQueryable<Company> expectedCompanies = CompaniesSampleData.Companies.OrderBy(x => x.Name)
            .Where(x => x.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
            .AsQueryable();
        GetCompaniesResult expectedResult = new()
        {
            Companies = new ListInfo<Company>
            {
                Data = expectedCompanies // ReSharper disable once UselessBinaryOperation
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList(),
                Count = expectedCompanies.Count()
            }
        };
        GetCompaniesQuery query = new()
        {
            ListParameters = new ListParameters
            {
                Pagination = new Pagination
                {
                    Page = page,
                    PageSize = pageSize
                },
                Sorting = new Sorting
                {
                    FieldName = sortingFieldName,
                    Order = sortingOrder
                },
                Search = new Search
                {
                    Query = searchQuery
                }
            }
        };
        GetCompaniesResult result = await _mediator.Send(query);
        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrderingFor(x => x.Companies.Data));
    }
}
