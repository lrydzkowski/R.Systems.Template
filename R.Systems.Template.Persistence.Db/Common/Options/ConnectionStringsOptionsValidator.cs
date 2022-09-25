using FluentValidation;

namespace R.Systems.Template.Persistence.Db.Common.Options;

internal class ConnectionStringsOptionsValidator : AbstractValidator<ConnectionStringsOptions>
{
    public ConnectionStringsOptionsValidator()
    {
        RuleFor(x => x.AppDb).NotEmpty()
            .WithName("AppDb")
            .OverridePropertyName($"{ConnectionStringsOptions.Position}.AppDb");
    }
}
