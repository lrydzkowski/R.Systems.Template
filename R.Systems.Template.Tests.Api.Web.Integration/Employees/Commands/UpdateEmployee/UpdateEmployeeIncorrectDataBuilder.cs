using System.Net;
using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Builders;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Commands.UpdateEmployee;

internal static class UpdateEmployeeIncorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();
        int employeeId = (int)EmployeesSampleData.Data[0].Id!;
        int companyId = (int)CompaniesSampleData.Data["Meta"].Id!;
        string? firstNameAttemptedValue = "";
        yield return BuildParameters(
            1,
            employeeId,
            new UpdateEmployeeRequest
                { FirstName = firstNameAttemptedValue, LastName = faker.Name.LastName(), CompanyId = companyId },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("FirstName", firstNameAttemptedValue) }
        );
        firstNameAttemptedValue = "  ";
        yield return BuildParameters(
            2,
            employeeId,
            new UpdateEmployeeRequest
                { FirstName = firstNameAttemptedValue, LastName = faker.Name.LastName(), CompanyId = companyId },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("FirstName", firstNameAttemptedValue) }
        );
        firstNameAttemptedValue = null;
        yield return BuildParameters(
            3,
            employeeId,
            new UpdateEmployeeRequest
                { FirstName = firstNameAttemptedValue, LastName = faker.Name.LastName(), CompanyId = companyId },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("FirstName", firstNameAttemptedValue) }
        );
        firstNameAttemptedValue = faker.Random.String2(101);
        yield return BuildParameters(
            4,
            employeeId,
            new UpdateEmployeeRequest
                { FirstName = firstNameAttemptedValue, LastName = faker.Name.LastName(), CompanyId = companyId },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildTooLongFieldValidationError("FirstName", 100, firstNameAttemptedValue) }
        );
        string? lastNameAttemptedValue = "";
        yield return BuildParameters(
            5,
            employeeId,
            new UpdateEmployeeRequest
                { FirstName = faker.Name.FirstName(), LastName = lastNameAttemptedValue, CompanyId = companyId },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("LastName", lastNameAttemptedValue) }
        );
        lastNameAttemptedValue = "  ";
        yield return BuildParameters(
            6,
            employeeId,
            new UpdateEmployeeRequest
                { FirstName = faker.Name.FirstName(), LastName = lastNameAttemptedValue, CompanyId = companyId },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("LastName", lastNameAttemptedValue) }
        );
        lastNameAttemptedValue = null;
        yield return BuildParameters(
            7,
            employeeId,
            new UpdateEmployeeRequest
                { FirstName = faker.Name.FirstName(), LastName = lastNameAttemptedValue, CompanyId = companyId },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("LastName", lastNameAttemptedValue) }
        );
        lastNameAttemptedValue = faker.Random.String2(101);
        yield return BuildParameters(
            8,
            employeeId,
            new UpdateEmployeeRequest
                { FirstName = faker.Name.FirstName(), LastName = lastNameAttemptedValue, CompanyId = companyId },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildTooLongFieldValidationError("LastName", 100, lastNameAttemptedValue) }
        );
        int companyIdAttemptedValue = 998;
        yield return BuildParameters(
            9,
            employeeId,
            new UpdateEmployeeRequest
            {
                FirstName = faker.Name.FirstName(), LastName = faker.Name.LastName(),
                CompanyId = companyIdAttemptedValue
            },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                new()
                {
                    PropertyName = "Company",
                    ErrorMessage = $"Company with the given id doesn't exist ('{companyIdAttemptedValue}').",
                    ErrorCode = "NotExist", AttemptedValue = companyIdAttemptedValue
                }
            }
        );
        int employeeIdAttemptedValue = 999;
        yield return BuildParameters(
            10,
            employeeIdAttemptedValue,
            new UpdateEmployeeRequest
                { FirstName = faker.Name.FirstName(), LastName = faker.Name.LastName(), CompanyId = companyId },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
            {
                new()
                {
                    PropertyName = "Employee",
                    ErrorMessage = $"Employee with the given id doesn't exist ('{employeeIdAttemptedValue}').",
                    ErrorCode = "NotExist", AttemptedValue = employeeIdAttemptedValue
                }
            }
        );
    }

    private static object[] BuildParameters(
        int id,
        int employeeId,
        UpdateEmployeeRequest data,
        HttpStatusCode expectedHttpStatus,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[]
        {
            id,
            employeeId,
            data,
            expectedHttpStatus,
            validationFailures
        };
    }
}
