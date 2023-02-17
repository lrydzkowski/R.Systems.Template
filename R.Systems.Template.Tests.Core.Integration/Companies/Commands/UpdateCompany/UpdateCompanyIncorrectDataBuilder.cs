using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Tests.Core.Integration.Common.Builders;
using R.Systems.Template.Tests.Core.Integration.Common.Db;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Commands.UpdateCompany;

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
                new UpdateCompanyCommand
                {
                    CompanyId = companyId,
                    Name = ""
                },
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(fieldName: "Name")
                }
            ),
            BuildParameters(
                2,
                new UpdateCompanyCommand
                {
                    CompanyId = companyId,
                    Name = "  "
                },
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(fieldName: "Name")
                }
            ),
            BuildParameters(
                3,
                new UpdateCompanyCommand
                {
                    CompanyId = companyId,
                    Name = null
                },
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildEmptyFieldValidationError(fieldName: "Name")
                }
            ),
            BuildParameters(
                4,
                new UpdateCompanyCommand
                {
                    CompanyId = companyId,
                    Name = faker.Random.String2(201)
                },
                new List<ValidationFailure>
                {
                    ValidationFailureBuilder.BuildTooLongFieldValidationError(fieldName: "Name", maxLength: 200)
                }
            ),
            BuildParameters(
                5,
                new UpdateCompanyCommand
                {
                    CompanyId = 999,
                    Name = faker.Company.CompanyName()
                },
                new List<ValidationFailure>
                {
                    new()
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
        UpdateCompanyCommand data,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[] { id, data, validationFailures };
    }
}
