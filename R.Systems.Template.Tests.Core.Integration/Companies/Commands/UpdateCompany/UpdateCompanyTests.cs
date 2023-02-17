using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Tests.Core.Integration.Common;
using R.Systems.Template.Tests.Core.Integration.Common.Db;
using R.Systems.Template.Tests.Core.Integration.Common.TestsCollections;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Commands.UpdateCompany;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class UpdateCompanyTests
{
    public UpdateCompanyTests(ITestOutputHelper output, SystemUnderTest<SampleDataDbInitializer> systemUnderTest)
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
        nameof(UpdateCompanyIncorrectDataBuilder.Build),
        MemberType = typeof(UpdateCompanyIncorrectDataBuilder)
    )]
    public async Task UpdateCompany_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        UpdateCompanyCommand command,
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
    [MemberData(nameof(UpdateCompanyCorrectDataBuilder.Build), MemberType = typeof(UpdateCompanyCorrectDataBuilder))]
    public async Task UpdateCompany_ShouldUpdateCompany_WhenDataIsCorrect(
        int id,
        UpdateCompanyCommand command
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        UpdateCompanyResult expectedResult = new()
        {
            Company = new Company
            {
                CompanyId = command.CompanyId,
                Name = command.Name!,
                Employees = new List<Employee>()
            }
        };

        UpdateCompanyResult result = await Mediator.Send(command);

        result.Should().BeEquivalentTo(expectedResult);
    }
}
