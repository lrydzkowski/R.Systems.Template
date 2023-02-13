using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Builders;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Companies.Commands.CreateCompany;

internal static class CreateCompanyIncorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();

        return new List<object[]>
        {
            BuildParameters(
                1,
                new CreateCompanyRequest
                {
                    Name = ""
                },
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(fieldName: "Name")
                }
            ),
            BuildParameters(
                2,
                new CreateCompanyRequest
                {
                    Name = "  "
                },
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(fieldName: "Name")
                }
            ),
            BuildParameters(
                3,
                new CreateCompanyRequest
                {
                    Name = null
                },
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(fieldName: "Name")
                }
            ),
            BuildParameters(
                4,
                new CreateCompanyRequest
                {
                    Name = faker.Random.String2(201)
                },
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildTooLongFieldValidationError(fieldName: "Name", maxLength: 200)
                }
            )
        };
    }

    private static object[] BuildParameters(
        int id,
        CreateCompanyRequest data,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[] { id, data, validationFailures };
    }
}
