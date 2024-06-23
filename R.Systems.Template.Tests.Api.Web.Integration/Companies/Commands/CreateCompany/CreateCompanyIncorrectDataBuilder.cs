using System.Net;
using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Builders;

namespace R.Systems.Template.Tests.Api.Web.Integration.Companies.Commands.CreateCompany;

internal static class CreateCompanyIncorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();
        string? nameAttemptedValue = "";
        yield return BuildParameters(
            1,
            new CreateCompanyCommand { Name = nameAttemptedValue },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );
        nameAttemptedValue = "  ";
        yield return BuildParameters(
            2,
            new CreateCompanyCommand { Name = nameAttemptedValue.Trim() },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue.Trim()) }
        );
        nameAttemptedValue = null;
        yield return BuildParameters(
            3,
            new CreateCompanyCommand { Name = nameAttemptedValue },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );
        nameAttemptedValue = faker.Random.String2(201);
        yield return BuildParameters(
            4,
            new CreateCompanyCommand { Name = nameAttemptedValue },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildTooLongFieldValidationError("Name", 200, nameAttemptedValue) }
        );
    }

    private static object[] BuildParameters(
        int id,
        CreateCompanyCommand data,
        HttpStatusCode expectedHttpStatus,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[]
        {
            id,
            data,
            expectedHttpStatus,
            validationFailures
        };
    }
}
