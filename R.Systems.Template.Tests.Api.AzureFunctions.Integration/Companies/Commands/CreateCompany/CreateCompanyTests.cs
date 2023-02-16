using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Function;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.TestsCollections;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using R.Systems.Template.Api.AzureFunctions.Models;
using Xunit.Abstractions;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Companies.Commands.CreateCompany;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class CreateCompanyTests
{
    public CreateCompanyTests(ITestOutputHelper output, FunctionFactory<SampleDataDbInitializer> functionFactory)
    {
        Output = output;
        Mapper = functionFactory.Services!.GetRequiredService<IMapper>();
        Mediator = functionFactory.Services!.GetRequiredService<ISender>();
    }

    private ITestOutputHelper Output { get; }
    private IMapper Mapper { get; }
    private ISender Mediator { get; }

    [Theory]
    [MemberData(
        nameof(CreateCompanyIncorrectDataBuilder.Build),
        MemberType = typeof(CreateCompanyIncorrectDataBuilder)
    )]
    public async Task CreateCompany_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        CreateCompanyRequest request,
        IEnumerable<ValidationFailure> validationFailures
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        CreateCompanyCommand command = Mapper.Map<CreateCompanyCommand>(request);

        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(command));

        exception.Errors.Should()
            .BeEquivalentTo(
                validationFailures,
                options => options.Including(x => x.PropertyName)
                    .Including(x => x.ErrorMessage)
                    .Including(x => x.ErrorCode)
            );
    }

    [Theory]
    [MemberData(nameof(CreateCompanyCorrectDataBuilder.Build), MemberType = typeof(CreateCompanyCorrectDataBuilder))]
    public async Task CreateCompany_ShouldCreateCompany_WhenDataIsCorrect(int id, CreateCompanyRequest request)
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        CreateCompanyResult expectedResult = new()
        {
            Company = new Company
            {
                CompanyId = CompaniesSampleData.Companies.Max(x => x.CompanyId) + 1,
                Name = request.Name!,
                Employees = new List<Employee>()
            }
        };

        CreateCompanyCommand command = Mapper.Map<CreateCompanyCommand>(request);
        CreateCompanyResult result = await Mediator.Send(command);

        result.Should().BeEquivalentTo(expectedResult);
    }
}
