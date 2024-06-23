namespace R.Systems.Template.Core.Common.Errors;

public class NotFoundException : Exception
{
    private IEnumerable<ErrorInfo> _errors;

    public NotFoundException(string message, ErrorInfo error) : this(message, new[] { error })
    {
    }

    public NotFoundException(string message, IEnumerable<ErrorInfo> errors) : base(message)
    {
        _errors = errors;
    }
}
