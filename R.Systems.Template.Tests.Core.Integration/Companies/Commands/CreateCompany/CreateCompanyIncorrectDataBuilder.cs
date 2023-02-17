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

        return new List<object[]>
        {
            BuildParameters(
                1,
                new CreateCompanyCommand
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
                new CreateCompanyCommand
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
                new CreateCompanyCommand
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
                new CreateCompanyCommand
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
        CreateCompanyCommand data,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[] { id, data, validationFailures };
    }
}
