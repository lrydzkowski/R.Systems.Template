using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Tests.Core.Integration.Common.Builders;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Commands.CreateCompany;

internal static class CreateCompanyIncorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();
        string? nameAttemptedValue = "";
        yield return BuildParameters(
            1,
            new CreateCompanyCommand { Name = nameAttemptedValue },
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );
        nameAttemptedValue = "  ";
        yield return BuildParameters(
            2,
            new CreateCompanyCommand { Name = nameAttemptedValue },
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );
        nameAttemptedValue = null;
        yield return BuildParameters(
            3,
            new CreateCompanyCommand { Name = nameAttemptedValue },
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );
        nameAttemptedValue = faker.Random.String2(201);
        yield return BuildParameters(
            4,
            new CreateCompanyCommand { Name = nameAttemptedValue },
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildTooLongFieldValidationError("Name", 200, nameAttemptedValue) }
        );
    }

    private static object[] BuildParameters(
        int id,
        CreateCompanyCommand data,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[]
        {
            id,
            data,
            validationFailures
        };
    }
}
