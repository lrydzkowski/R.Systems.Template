using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using R.Systems.Template.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Template.Infrastructure.Db.SqlServer.Companies.Commands;

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

        if (postgresException.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            if (postgresException.ConstraintName == "IX_company_name")
            {
                errors.Add(
                    new ValidationFailure
                    {
                        PropertyName = "Name",
                        ErrorMessage = $"Company with the given name already exists ('{companyEntity.Name}').",
                        AttemptedValue = companyEntity.Name,
                        ErrorCode = "Exists"
                    }
                );
            }
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}
