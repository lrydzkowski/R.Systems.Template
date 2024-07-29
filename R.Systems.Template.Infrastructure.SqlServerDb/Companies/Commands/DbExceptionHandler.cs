using FluentValidation;
using FluentValidation.Results;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Infrastructure.SqlServerDb.Common.Entities;

namespace R.Systems.Template.Infrastructure.SqlServerDb.Companies.Commands;

internal class DbExceptionHandler
{
    public void Handle(DbUpdateException dbUpdateException, CompanyEntity companyEntity)
    {
        if (dbUpdateException.InnerException is not SqlException sqlException)
        {
            return;
        }

        if ((sqlException.Number == 2601 || sqlException.Number == 2627)
            && sqlException.Message.Contains("IX_company_name"))
        {
            throw new ValidationException(
                [
                    new ValidationFailure
                    {
                        PropertyName = "Name",
                        ErrorMessage = $"Company with the given name already exists ('{companyEntity.Name}').",
                        AttemptedValue = companyEntity.Name,
                        ErrorCode = "UniquenessValidator"
                    }
                ]
            );
        }
    }
}
