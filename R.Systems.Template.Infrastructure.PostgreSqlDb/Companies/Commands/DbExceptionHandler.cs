using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Companies.Commands;

internal class DbExceptionHandler
{
    public void Handle(DbUpdateException dbUpdateException, CompanyEntity companyEntity)
    {
        if (dbUpdateException.InnerException == null
            || dbUpdateException.InnerException is not PostgresException postgresException)
        {
            return;
        }

        List<ValidationFailure> errors = new();
        if (postgresException is { SqlState: PostgresErrorCodes.UniqueViolation, ConstraintName: "IX_company_name" })
        {
            errors.Add(
                new ValidationFailure
                {
                    PropertyName = "Name",
                    ErrorMessage = $"Company with the given name already exists ('{companyEntity.Name}').",
                    AttemptedValue = companyEntity.Name,
                    ErrorCode = "UniquenessValidator"
                }
            );
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}
