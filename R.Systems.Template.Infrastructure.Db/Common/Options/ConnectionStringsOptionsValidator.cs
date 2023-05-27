using FluentValidation;

namespace R.Systems.Template.Infrastructure.Db.Common.Options;

internal class ConnectionStringsOptionsValidator : AbstractValidator<ConnectionStringsOptions>
{
    public ConnectionStringsOptionsValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.AppSqlServerDb) || !string.IsNullOrWhiteSpace(x.AppPostgresDb))
            .WithMessage("123");
    }
}
