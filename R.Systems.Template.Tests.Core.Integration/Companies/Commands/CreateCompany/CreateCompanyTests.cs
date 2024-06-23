using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Tests.Core.Integration.Common;
using R.Systems.Template.Tests.Core.Integration.Common.Db;
using R.Systems.Template.Tests.Core.Integration.Common.TestsCollections;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Commands.CreateCompany;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class CreateCompanyTests
{
    private readonly ISender _mediator;

    private readonly ITestOutputHelper _output;

    public CreateCompanyTests(ITestOutputHelper output, SystemUnderTest<SampleDataDbInitializer> systemUnderTest)
    {
        _output = output;
        _mediator = systemUnderTest.BuildServiceProvider().GetRequiredService<ISender>();
    }

    [Theory]
    [MemberData(
        nameof(CreateCompanyIncorrectDataBuilder.Build),
        MemberType = typeof(CreateCompanyIncorrectDataBuilder)
    )]
    public async Task CreateCompany_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        CreateCompanyCommand command,
        IEnumerable<ValidationFailure> validationFailures
    )
    {
        _output.WriteLine("Parameters set with id = {0}", id);
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command));
        exception.Errors.Should()
            .BeEquivalentTo(
                validationFailures,
                options => options.WithStrictOrdering()
                    .Including(x => x.AttemptedValue)
                    .Including(x => x.ErrorCode)
                    .Including(x => x.ErrorMessage)
                    .Including(x => x.PropertyName)
            );
    }

    [Theory]
    [MemberData(nameof(CreateCompanyCorrectDataBuilder.Build), MemberType = typeof(CreateCompanyCorrectDataBuilder))]
    public async Task CreateCompany_ShouldCreateCompany_WhenDataIsCorrect(int id, CreateCompanyCommand command)
    {
        _output.WriteLine("Parameters set with id = {0}", id);
        CreateCompanyResult expectedResult = new()
        {
            Company = new Company
            {
                Name = command.Name!
            }
        };
        CreateCompanyResult result = await _mediator.Send(command);
        result.Should().BeEquivalentTo(expectedResult, options => options.Excluding(x => x.Company.CompanyId));
    }
}
