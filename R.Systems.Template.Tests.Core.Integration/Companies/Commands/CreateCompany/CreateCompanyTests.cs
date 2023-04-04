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
    public CreateCompanyTests(ITestOutputHelper output, SystemUnderTest<SampleDataDbInitializer> systemUnderTest)
    {
        Output = output;
        SystemUnderTest = systemUnderTest;
        Mediator = SystemUnderTest.BuildServiceProvider().GetRequiredService<ISender>();
    }

    private ITestOutputHelper Output { get; }
    private SystemUnderTest<SampleDataDbInitializer> SystemUnderTest { get; }
    private ISender Mediator { get; }

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
        Output.WriteLine("Parameters set with id = {0}", id);

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
    public async Task CreateCompany_ShouldCreateCompany_WhenDataIsCorrect(int id, CreateCompanyCommand command)
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        CreateCompanyResult expectedResult = new()
        {
            Company = new Company
            {
                Name = command.Name!
            }
        };

        CreateCompanyResult result = await Mediator.Send(command);

        result.Should().BeEquivalentTo(expectedResult, options => options.Excluding(x => x.Company.CompanyId));
    }
}
