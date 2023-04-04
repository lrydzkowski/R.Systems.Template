using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Tests.Core.Integration.Common;
using R.Systems.Template.Tests.Core.Integration.Common.Db;
using R.Systems.Template.Tests.Core.Integration.Common.TestsCollections;
using System.Text.Json;
using Xunit.Abstractions;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Commands.DeleteCompany;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class DeleteCompanyTests
{
    public DeleteCompanyTests(ITestOutputHelper output, SystemUnderTest<SampleDataDbInitializer> systemUnderTest)
    {
        Output = output;
        SystemUnderTest = systemUnderTest;
        Mediator = SystemUnderTest.BuildServiceProvider().GetRequiredService<ISender>();
    }

    private ITestOutputHelper Output { get; }
    private SystemUnderTest<SampleDataDbInitializer> SystemUnderTest { get; }
    private ISender Mediator { get; }

    [Fact]
    public async Task DeleteCompany_ShouldReturnValidationError_WhenCompanyNotExist()
    {
        int companyId = int.MaxValue;
        List<ValidationFailure> expectedValidationFailures = new()
        {
            new ValidationFailure
            {
                PropertyName = "Company",
                ErrorMessage = $"Company with the given id doesn't exist ('{companyId}').",
                AttemptedValue = JsonSerializer.SerializeToElement(companyId),
                ErrorCode = "NotExist"
            }
        };
        DeleteCompanyCommand command = new() { CompanyId = companyId };
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(command));

        exception.Errors.Should()
            .BeEquivalentTo(
                expectedValidationFailures,
                options => options.Including(x => x.PropertyName)
                    .Including(x => x.ErrorMessage)
                    .Including(x => x.ErrorCode)
            );
    }

    [Fact]
    public async Task DeleteCompany_ShouldDeleteCompany_WhenCompanyExists()
    {
        CreateCompanyCommand createCompanyCommand = new()
        {
            Name = "Test Company"
        };
        CreateCompanyResult createCompanyResult = await Mediator.Send(createCompanyCommand);

        int companyId = createCompanyResult.Company.CompanyId;

        GetCompanyQuery getCompanyQuery = new() { CompanyId = companyId };
        GetCompanyResult getCompanyResult = await Mediator.Send(getCompanyQuery);

        DeleteCompanyCommand deleteCompanyCommand = new() { CompanyId = companyId };
        await Mediator.Send(deleteCompanyCommand);

        GetCompanyQuery getCompanyAfterDeleteQuery = new() { CompanyId = companyId };
        GetCompanyResult getCompanyAfterDeleteResult = await Mediator.Send(getCompanyAfterDeleteQuery);

        getCompanyResult.Company.Should().NotBeNull();
        getCompanyResult.Company?.Name.Should().Be(createCompanyCommand.Name);
        getCompanyAfterDeleteResult.Company.Should().BeNull();
    }
}
