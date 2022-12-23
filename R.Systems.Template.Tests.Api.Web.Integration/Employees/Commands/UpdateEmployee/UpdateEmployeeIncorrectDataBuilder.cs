using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Builders;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;
using System.Net;

namespace R.Systems.Template.Tests.Api.Web.Integration.Employees.Commands.UpdateEmployee;

internal static class UpdateEmployeeIncorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();
        int employeeId = (int)EmployeesSampleData.Data[0].Id!;
        int companyId = (int)CompaniesSampleData.Data["Meta"].Id!;

        return new List<object[]>
        {
            BuildParameters(
                1,
                employeeId,
                new UpdateEmployeeRequest
                {
                    FirstName = "",
                    LastName = faker.Name.LastName(),
                    CompanyId = companyId
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(
                        fieldName: "FirstName",
                        fieldNameInMsg: "First Name"
                    )
                }
            ),
            BuildParameters(
                2,
                employeeId,
                new UpdateEmployeeRequest
                {
                    FirstName = "  ",
                    LastName = faker.Name.LastName(),
                    CompanyId = companyId
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(
                        fieldName: "FirstName",
                        fieldNameInMsg: "First Name"
                    )
                }
            ),
            BuildParameters(
                3,
                employeeId,
                new UpdateEmployeeRequest
                {
                    FirstName = null,
                    LastName = faker.Name.LastName(),
                    CompanyId = companyId
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(
                        fieldName: "FirstName",
                        fieldNameInMsg: "First Name"
                    )
                }
            ),
            BuildParameters(
                4,
                employeeId,
                new UpdateEmployeeRequest
                {
                    FirstName = faker.Random.String2(101),
                    LastName = faker.Name.LastName(),
                    CompanyId = companyId
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildTooLongFieldValidationError(
                        fieldName: "FirstName",
                        maxLength: 100,
                        fieldNameInMsg: "First Name"
                    )
                }
            ),
            BuildParameters(
                5,
                employeeId,
                new UpdateEmployeeRequest
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = "",
                    CompanyId = companyId
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(
                        fieldName: "LastName",
                        fieldNameInMsg: "Last Name"
                    )
                }
            ),
            BuildParameters(
                6,
                employeeId,
                new UpdateEmployeeRequest
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = "  ",
                    CompanyId = companyId
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(
                        fieldName: "LastName",
                        fieldNameInMsg: "Last Name"
                    )
                }
            ),
            BuildParameters(
                7,
                employeeId,
                new UpdateEmployeeRequest
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = null,
                    CompanyId = companyId
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(
                        fieldName: "LastName",
                        fieldNameInMsg: "Last Name"
                    )
                }
            ),
            BuildParameters(
                8,
                employeeId,
                new UpdateEmployeeRequest
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Random.String2(101),
                    CompanyId = companyId
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildTooLongFieldValidationError(
                        fieldName: "LastName",
                        maxLength: 100,
                        fieldNameInMsg: "Last Name"
                    )
                }
            ),
            BuildParameters(
                9,
                employeeId,
                new UpdateEmployeeRequest
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    CompanyId = 999
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    new ValidationFailure
                    {
                        PropertyName = "Company",
                        ErrorMessage = "Company with the given id doesn't exist ('999').",
                        ErrorCode = "NotExist"
                    }
                }
            ),
            BuildParameters(
                10,
                999,
                new UpdateEmployeeRequest
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    CompanyId = companyId
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    new ValidationFailure
                    {
                        PropertyName = "Employee",
                        ErrorMessage = "Employee with the given id doesn't exist ('999').",
                        ErrorCode = "NotExist"
                    }
                }
            )
        };
    }

    private static object[] BuildParameters(
        int id,
        int employeeId,
        UpdateEmployeeRequest data,
        HttpStatusCode expectedHttpStatus,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[] { id, employeeId, data, expectedHttpStatus, validationFailures };
    }
}
