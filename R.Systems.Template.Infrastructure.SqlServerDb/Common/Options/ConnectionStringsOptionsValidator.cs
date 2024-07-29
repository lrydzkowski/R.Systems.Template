using FluentValidation;

namespace R.Systems.Template.Infrastructure.SqlServerDb.Common.Options;

internal class ConnectionStringsOptionsValidator : AbstractValidator<ConnectionStringsOptions>
{
    public ConnectionStringsOptionsValidator()
    {
        RuleFor(x => x.AppSqlServerDb)
            .NotEmpty()
            .WithName(nameof(ConnectionStringsOptions.AppSqlServerDb))
            .OverridePropertyName(
                $"{ConnectionStringsOptions.Position}.{nameof(ConnectionStringsOptions.AppSqlServerDb)}"
            );
    }
}
