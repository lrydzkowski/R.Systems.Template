using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Persistence.Db.Employees.Commands;

internal class EmployeeValidator
{
    public EmployeeValidator(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<Result<bool>> VerifyCompanyExistenceAsync(int companyId)
    {
        bool exist = await DbContext.Companies.AsNoTracking()
                         .Where(x => x.Id == companyId)
                         .Select(x => x.Id)
                         .FirstOrDefaultAsync()
                     != null;
        if (!exist)
        {
            ValidationException validationException = new(
                new List<ValidationFailure>
                {
                    new()
                    {
                        PropertyName = "Company",
                        ErrorMessage = $"Company with the given id doesn't exist ('{companyId}').",
                        AttemptedValue = companyId,
                        ErrorCode = "NotExist"
                    }
                }
            );

            return new Result<bool>(validationException);
        }

        return true;
    }
}
