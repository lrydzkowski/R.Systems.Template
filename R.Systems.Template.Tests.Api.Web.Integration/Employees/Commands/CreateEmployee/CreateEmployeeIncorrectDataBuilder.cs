using System.Net;
using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Builders;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Commands.CreateEmployee;

internal static class CreateEmployeeIncorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();

        string? firstNameAttemptedValue = "";
        yield return BuildParameters(
            1,
            new CreateEmployeeCommand
            {
                FirstName = firstNameAttemptedValue,
                LastName = faker.Name.LastName(),
                CompanyId = CompaniesSampleData.Data["Meta"].Id
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                ValidationFailureBuilder.BuildEmptyFieldValidationError(
                    fieldName: "FirstName",
                    attemptedValue: firstNameAttemptedValue
                )
            }
        );

        firstNameAttemptedValue = "  ";
        yield return BuildParameters(
            2,
            new CreateEmployeeCommand
            {
                FirstName = firstNameAttemptedValue,
                LastName = faker.Name.LastName(),
                CompanyId = CompaniesSampleData.Data["Meta"].Id
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                ValidationFailureBuilder.BuildEmptyFieldValidationError(
                    fieldName: "FirstName",
                    attemptedValue: firstNameAttemptedValue
                )
            }
        );

        firstNameAttemptedValue = null;
        yield return BuildParameters(
            3,
            new CreateEmployeeCommand
            {
                FirstName = firstNameAttemptedValue,
                LastName = faker.Name.LastName(),
                CompanyId = CompaniesSampleData.Data["Meta"].Id
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                ValidationFailureBuilder.BuildEmptyFieldValidationError(
                    fieldName: "FirstName",
                    attemptedValue: firstNameAttemptedValue
                )
            }
        );

        firstNameAttemptedValue = faker.Random.String2(101);
        yield return BuildParameters(
            4,
            new CreateEmployeeCommand
            {
                FirstName = firstNameAttemptedValue,
                LastName = faker.Name.LastName(),
                CompanyId = CompaniesSampleData.Data["Meta"].Id
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                ValidationFailureBuilder.BuildTooLongFieldValidationError(
                    fieldName: "FirstName",
                    maxLength: 100,
                    attemptedValue: firstNameAttemptedValue
                )
            }
        );

        string? lastNameAttemptedValue = "";
        yield return BuildParameters(
            5,
            new CreateEmployeeCommand
            {
                FirstName = faker.Name.FirstName(),
                LastName = lastNameAttemptedValue,
                CompanyId = CompaniesSampleData.Data["Meta"].Id
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                ValidationFailureBuilder.BuildEmptyFieldValidationError(
                    fieldName: "LastName",
                    attemptedValue: lastNameAttemptedValue
                )
            }
        );

        lastNameAttemptedValue = "  ";
        yield return BuildParameters(
            6,
            new CreateEmployeeCommand
            {
                FirstName = faker.Name.FirstName(),
                LastName = lastNameAttemptedValue,
                CompanyId = CompaniesSampleData.Data["Meta"].Id
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                ValidationFailureBuilder.BuildEmptyFieldValidationError(
                    fieldName: "LastName",
                    attemptedValue: lastNameAttemptedValue
                )
            }
        );

        lastNameAttemptedValue = null;
        yield return BuildParameters(
            7,
            new CreateEmployeeCommand
            {
                FirstName = faker.Name.FirstName(),
                LastName = lastNameAttemptedValue,
                CompanyId = CompaniesSampleData.Data["Meta"].Id
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                ValidationFailureBuilder.BuildEmptyFieldValidationError(
                    fieldName: "LastName",
                    attemptedValue: lastNameAttemptedValue
                )
            }
        );

        lastNameAttemptedValue = faker.Random.String2(101);
        yield return BuildParameters(
            8,
            new CreateEmployeeCommand
            {
                FirstName = faker.Name.FirstName(),
                LastName = lastNameAttemptedValue,
                CompanyId = CompaniesSampleData.Data["Meta"].Id
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                ValidationFailureBuilder.BuildTooLongFieldValidationError(
                    fieldName: "LastName",
                    maxLength: 100,
                    attemptedValue: lastNameAttemptedValue
                )
            }
        );

        int companyIdAttemptedValue = 999;
        yield return BuildParameters(
            9,
            new CreateEmployeeCommand
            {
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                CompanyId = companyIdAttemptedValue
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                new()
                {
                    PropertyName = "Company",
                    ErrorMessage = $"Company with the given id doesn't exist ('{companyIdAttemptedValue}').",
                    ErrorCode = "NotExist",
                    AttemptedValue = companyIdAttemptedValue
                }
            }
        );
    }

    private static object[] BuildParameters(
        int id,
        CreateEmployeeCommand data,
        HttpStatusCode expectedHttpStatus,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[] { id, data, expectedHttpStatus, validationFailures };
    }
}
