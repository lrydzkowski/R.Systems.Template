using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class DbExceptionHandler
{
    public List<ValidationFailure> Handle(DbUpdateException dbUpdateException, CompanyEntity companyEntity)
    {
        List<ValidationFailure> errors = new();

        if (dbUpdateException.InnerException == null
            || dbUpdateException.InnerException is not PostgresException postgresException)
        {
            return errors;
        }

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

        return errors;
    }
}
