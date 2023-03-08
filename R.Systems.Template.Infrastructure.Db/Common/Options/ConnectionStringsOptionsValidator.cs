using FluentValidation;

namespace R.Systems.Template.Infrastructure.Db.Common.Options;

internal class ConnectionStringsOptionsValidator : AbstractValidator<ConnectionStringsOptions>
{
    public ConnectionStringsOptionsValidator()
    {
        RuleFor(x => x.AppDb)
            .NotEmpty()
            .WithName("AppDb")
            .OverridePropertyName($"{ConnectionStringsOptions.Position}.{nameof(ConnectionStringsOptions.AppDb)}");
    }
}
