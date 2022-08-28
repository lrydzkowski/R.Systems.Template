using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Db.SampleData;
using R.Systems.Template.WebApi.Api;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Companies.Commands.UpdateCompany;

internal static class UpdateCompanyIncorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();
        int companyId = (int)CompaniesSampleData.Data["Meta"].Id!;

        return new List<object[]>
        {
            BuildParameters(
                1,
                companyId,
                new UpdateCompanyRequest
                {
                    Name = ""
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(fieldName: "Name")
                }
            ),
            BuildParameters(
                2,
                companyId,
                new UpdateCompanyRequest
                {
                    Name = "  "
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(fieldName: "Name")
                }
            ),
            BuildParameters(
                3,
                companyId,
                new UpdateCompanyRequest
                {
                    Name = null
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(fieldName: "Name")
                }
            ),
            BuildParameters(
                4,
                companyId,
                new UpdateCompanyRequest
                {
                    Name = faker.Random.String2(201)
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildTooLongFieldValidationError(fieldName: "Name", maxLength: 200)
                }
            ),
            BuildParameters(
                5,
                999,
                new UpdateCompanyRequest
                {
                    Name = faker.Company.CompanyName()
                },
                HttpStatusCode.UnprocessableEntity,
                new List<ValidationFailure>
                {
                    new ValidationFailure()
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
        int companyId,
        UpdateCompanyRequest data,
        HttpStatusCode expectedHttpStatus,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[] { id, companyId, data, expectedHttpStatus, validationFailures };
    }
}
