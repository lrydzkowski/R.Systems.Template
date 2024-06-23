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
        string? nameAttemptedValue = "";
        yield return BuildParameters(
            1,
            new UpdateCompanyCommand { CompanyId = companyId, Name = nameAttemptedValue },
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );
        nameAttemptedValue = "  ";
        yield return BuildParameters(
            2,
            new UpdateCompanyCommand { CompanyId = companyId, Name = nameAttemptedValue },
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );
        nameAttemptedValue = null;
        yield return BuildParameters(
            3,
            new UpdateCompanyCommand { CompanyId = companyId, Name = nameAttemptedValue },
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );
        nameAttemptedValue = faker.Random.String2(201);
        yield return BuildParameters(
            4,
            new UpdateCompanyCommand { CompanyId = companyId, Name = nameAttemptedValue },
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildTooLongFieldValidationError("Name", 200, nameAttemptedValue) }
        );
        int companyIdAttemptedValue = 999;
        yield return BuildParameters(
            5,
            new UpdateCompanyCommand { CompanyId = companyIdAttemptedValue, Name = faker.Company.CompanyName() },
            new List<ValidationFailure>
            {
                new()
                {
                    AttemptedValue = companyIdAttemptedValue, PropertyName = "Company",
                    ErrorMessage = $"Company with the given id doesn't exist ('{companyIdAttemptedValue}').",
                    ErrorCode = "NotExist"
                }
            }
        );
    }

    private static object[] BuildParameters(
        int id,
        UpdateCompanyCommand data,
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
