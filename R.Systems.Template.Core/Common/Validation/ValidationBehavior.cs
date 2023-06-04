using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace R.Systems.Template.Core.Common.Validation;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        Validators = validators;
    }

    private IEnumerable<IValidator<TRequest>> Validators { get; }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!Validators.Any())
        {
            return await next();
        }

        List<ValidationFailure> validationFailures = new();
        ValidationContext<TRequest> context = new(request);
        foreach (IValidator<TRequest> validator in Validators)
        {
            ValidationResult result = await validator.ValidateAsync(context, cancellationToken);
            if (result.Errors.Count == 0)
            {
                continue;
            }

            validationFailures.AddRange(result.Errors);
        }

        if (validationFailures.Count > 0)
        {
            throw new ValidationException(validationFailures);
        }

        return await next();
    }
}
