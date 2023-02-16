using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
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

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Companies.Commands.UpdateCompany;

[Collection(CommandTestsCollection.CollectionName)]
[Trait(TestConstants.Category, CommandTestsCollection.CollectionName)]
public class UpdateCompanyTests
{
    public UpdateCompanyTests(ITestOutputHelper output, FunctionFactory<SampleDataDbInitializer> functionFactory)
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
        nameof(UpdateCompanyIncorrectDataBuilder.Build),
        MemberType = typeof(UpdateCompanyIncorrectDataBuilder)
    )]
    public async Task UpdateCompany_ShouldReturnValidationErrors_WhenDataIsIncorrect(
        int id,
        int companyId,
        UpdateCompanyRequest request,
        IEnumerable<ValidationFailure> validationFailures
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        UpdateCompanyCommand command = Mapper.Map<UpdateCompanyCommand>(request);
        command.CompanyId = companyId;

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
        int companyId,
        UpdateCompanyRequest request
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        UpdateCompanyResult expectedResult = new()
        {
            Company = new Company
            {
                CompanyId = companyId,
                Name = request.Name!,
                Employees = new List<Employee>()
            }
        };

        UpdateCompanyCommand command = Mapper.Map<UpdateCompanyCommand>(request);
        command.CompanyId = companyId;
        UpdateCompanyResult result = await Mediator.Send(command);

        result.Should().BeEquivalentTo(expectedResult);
    }
}
