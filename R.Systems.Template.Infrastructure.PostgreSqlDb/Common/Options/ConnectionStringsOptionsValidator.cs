using FluentValidation;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Options;

internal class ConnectionStringsOptionsValidator : AbstractValidator<ConnectionStringsOptions>
{
    public ConnectionStringsOptionsValidator()
    {
        RuleFor(x => x.AppPostgreSqlDb)
            .NotEmpty()
            .WithName(nameof(ConnectionStringsOptions.AppPostgreSqlDb))
            .OverridePropertyName(
                $"{ConnectionStringsOptions.Position}.{nameof(ConnectionStringsOptions.AppPostgreSqlDb)}"
            );
    }
}
