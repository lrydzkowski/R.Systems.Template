using FluentValidation;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Options;

internal class ConnectionStringsOptionsValidator : AbstractValidator<ConnectionStringsOptions>
{
    public ConnectionStringsOptionsValidator()
    {
        RuleFor(x => x.MongoDb)
            .NotEmpty()
            .WithName(nameof(ConnectionStringsOptions.MongoDb))
            .OverridePropertyName(
                $"{ConnectionStringsOptions.Position}.{nameof(ConnectionStringsOptions.MongoDb)}"
            );
    }
}
