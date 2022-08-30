using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Employees.Commands.CreateEmployee;

internal static class CreateEmployeeIncorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();

        return new List<object[]>
        {
            BuildParameters(
                1,
                new CreateEmployeeCommand
                {
                    FirstName = "",
                    LastName = faker.Name.LastName(),
                    CompanyId = CompaniesSampleData.Data["Meta"].Id
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
                new CreateEmployeeCommand
                {
                    FirstName = "  ",
                    LastName = faker.Name.LastName(),
                    CompanyId = CompaniesSampleData.Data["Meta"].Id
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
                new CreateEmployeeCommand
                {
                    FirstName = null,
                    LastName = faker.Name.LastName(),
                    CompanyId = CompaniesSampleData.Data["Meta"].Id
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
                new CreateEmployeeCommand
                {
                    FirstName = faker.Random.String2(101),
                    LastName = faker.Name.LastName(),
                    CompanyId = CompaniesSampleData.Data["Meta"].Id
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
                new CreateEmployeeCommand
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = "",
                    CompanyId = CompaniesSampleData.Data["Meta"].Id
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
                new CreateEmployeeCommand
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = "  ",
                    CompanyId = CompaniesSampleData.Data["Meta"].Id
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
                new CreateEmployeeCommand
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = null,
                    CompanyId = CompaniesSampleData.Data["Meta"].Id
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
                new CreateEmployeeCommand
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Random.String2(101),
                    CompanyId = CompaniesSampleData.Data["Meta"].Id
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
                new CreateEmployeeCommand
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
            )
        };
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
