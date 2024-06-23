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

namespace R.Systems.Template.Tests.Core.Integration.Companies.Commands.DeleteCompany;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class DeleteCompanyTests
{
    private readonly ISender _mediator;

    private readonly SystemUnderTest<SampleDataDbInitializer> _systemUnderTest;

    public DeleteCompanyTests(SystemUnderTest<SampleDataDbInitializer> systemUnderTest)
    {
        _systemUnderTest = systemUnderTest;
        _mediator = _systemUnderTest.BuildServiceProvider().GetRequiredService<ISender>();
    }

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
                AttemptedValue = companyId,
                ErrorCode = "NotExist"
            }
        };
        DeleteCompanyCommand command = new()
        {
            CompanyId = companyId
        };
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command));
        exception.Errors.Should().BeEquivalentTo(expectedValidationFailures, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task DeleteCompany_ShouldDeleteCompany_WhenCompanyExists()
    {
        CreateCompanyCommand createCompanyCommand = new()
        {
            Name = "Test Company"
        };
        CreateCompanyResult createCompanyResult = await _mediator.Send(createCompanyCommand);
        int companyId = createCompanyResult.Company.CompanyId;
        GetCompanyQuery getCompanyQuery = new()
        {
            CompanyId = companyId
        };
        GetCompanyResult getCompanyResult = await _mediator.Send(getCompanyQuery);
        DeleteCompanyCommand deleteCompanyCommand = new()
        {
            CompanyId = companyId
        };
        await _mediator.Send(deleteCompanyCommand);
        GetCompanyQuery getCompanyAfterDeleteQuery = new()
        {
            CompanyId = companyId
        };
        GetCompanyResult getCompanyAfterDeleteResult = await _mediator.Send(getCompanyAfterDeleteQuery);
        getCompanyResult.Company.Should().NotBeNull();
        getCompanyResult.Company?.Name.Should().Be(createCompanyCommand.Name);
        getCompanyAfterDeleteResult.Company.Should().BeNull();
    }
}
