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
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next
    )
    {
        if (!Validators.Any())
        {
            return await next();
        }

        ValidationContext<TRequest> context = new(request);
        List<ValidationFailure> validationFailures = Validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToList();
        if (validationFailures.Count > 0)
        {
            ValidationException validationException = new(validationFailures);

            var responseType = typeof(TResponse);
            var resultType = responseType.GetGenericArguments().FirstOrDefault();
            if (resultType == null)
            {
                throw new InvalidOperationException("Mediatr return type has to by Result<T>.");
            }

            var invalidResponseType = typeof(Result<>).MakeGenericType(resultType);
            var resultInstance = Activator.CreateInstance(
                invalidResponseType,
                validationException
            );
            if (resultInstance == null)
            {
                throw new InvalidOperationException("It was impossible to create Result<T> instance.");
            }

            return (TResponse)resultInstance;
        }

        return await next();
    }
}
