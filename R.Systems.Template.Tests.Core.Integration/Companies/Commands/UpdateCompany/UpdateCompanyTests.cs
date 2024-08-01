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
    private readonly ISender _mediator;

    private readonly ITestOutputHelper _output;

    public UpdateCompanyTests(ITestOutputHelper output, SystemUnderTest<SampleDataDbInitializer> systemUnderTest)
    {
        _output = output;
        _mediator = systemUnderTest.BuildServiceProvider().GetRequiredService<ISender>();
    }

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
    [MemberData(nameof(UpdateCompanyCorrectDataBuilder.Build), MemberType = typeof(UpdateCompanyCorrectDataBuilder))]
    public async Task UpdateCompany_ShouldUpdateCompany_WhenDataIsCorrect(int id, UpdateCompanyCommand command)
    {
        _output.WriteLine("Parameters set with id = {0}", id);
        UpdateCompanyResult expectedResult = new()
        {
            Company = new Company
            {
                CompanyId = Guid.Parse(command.CompanyId),
                Name = command.Name!
            }
        };
        UpdateCompanyResult result = await _mediator.Send(command);
        result.Should().BeEquivalentTo(expectedResult);
    }
}
