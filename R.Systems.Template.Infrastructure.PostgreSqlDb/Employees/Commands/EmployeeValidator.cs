using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Employees.Commands;

internal class EmployeeValidator
{
    private readonly AppDbContext _dbContext;

    public EmployeeValidator(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task VerifyCompanyExistenceAsync(Guid companyId)
    {
        bool exist = await _dbContext.Companies.AsNoTracking()
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
                        AttemptedValue = companyId, ErrorCode = "NotExist"
                    }
                }
            );
        }
    }
}
