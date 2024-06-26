using FluentValidation;

namespace R.Systems.Template.Infrastructure.Db.Common.Options;

internal class ConnectionStringsOptionsValidator : AbstractValidator<ConnectionStringsOptions>
{
    public ConnectionStringsOptionsValidator()
    {
        RuleFor(x => x.AppPostgresDb)
            .NotEmpty()
            .WithName(nameof(ConnectionStringsOptions.AppPostgresDb))
            .OverridePropertyName(
                $"{ConnectionStringsOptions.Position}.{nameof(ConnectionStringsOptions.AppPostgresDb)}"
            );
    }
}
