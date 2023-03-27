using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace R.Systems.Template.Infrastructure.Db.Postgres.Employees.Commands;

internal class EmployeeValidator
{
    public EmployeeValidator(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task VerifyCompanyExistenceAsync(int companyId)
    {
        bool exist = await DbContext.Companies.AsNoTracking()
                         .Where(x => x.Id == companyId)
                         .Select(x => x.Id)
                         .FirstOrDefaultAsync()
                     != null;
        if (!exist)
        {
            throw new ValidationException(
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
        }
    }
}
