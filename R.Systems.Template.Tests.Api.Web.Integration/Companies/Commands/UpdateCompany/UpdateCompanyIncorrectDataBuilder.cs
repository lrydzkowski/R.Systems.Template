using System.Net;
using Bogus;
using FluentValidation.Results;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Builders;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

namespace R.Systems.Template.Tests.Api.Web.Integration.Companies.Commands.UpdateCompany;

internal static class UpdateCompanyIncorrectDataBuilder
{
    public static IEnumerable<object[]> Build()
    {
        Faker faker = new();
        string companyId = CompaniesSampleData.Data["Meta"].Id.ToString()!;

        string? nameAttemptedValue = "";
        yield return BuildParameters(
            1,
            companyId,
            new UpdateCompanyRequest { Name = nameAttemptedValue },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );

        nameAttemptedValue = "  ";
        yield return BuildParameters(
            2,
            companyId,
            new UpdateCompanyRequest { Name = nameAttemptedValue },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );

        nameAttemptedValue = null;
        yield return BuildParameters(
            3,
            companyId,
            new UpdateCompanyRequest { Name = nameAttemptedValue },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildEmptyFieldValidationError("Name", nameAttemptedValue) }
        );

        nameAttemptedValue = faker.Random.String2(201);
        yield return BuildParameters(
            4,
            companyId,
            new UpdateCompanyRequest { Name = nameAttemptedValue },
            HttpStatusCode.UnprocessableEntity,
            new List<ValidationFailure>
                { ValidationFailureBuilder.BuildTooLongFieldValidationError("Name", 200, nameAttemptedValue) }
        );

        string companyIdAttemptedValue = "7944917a-b626-465e-b8be-6aa50a6fd9fa";
        yield return BuildParameters(
            5,
            companyIdAttemptedValue,
            new UpdateCompanyRequest { Name = faker.Company.CompanyName() },
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
    }

    private static object[] BuildParameters(
        int id,
        string companyId,
        UpdateCompanyRequest data,
        HttpStatusCode expectedHttpStatus,
        List<ValidationFailure> validationFailures
    )
    {
        return new object[]
        {
            id,
            companyId,
            data,
            expectedHttpStatus,
            validationFailures
        };
    }
}
