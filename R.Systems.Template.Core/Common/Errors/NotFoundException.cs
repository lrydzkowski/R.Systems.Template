namespace R.Systems.Template.Core.Common.Errors;

public class NotFoundException : Exception
{
    public NotFoundException(string message, ErrorInfo error) : this(message, new[] { error })
    {
    }

    public NotFoundException(string message, IEnumerable<ErrorInfo> errors) : base(message)
    {
        Errors = errors;
    }

    public IEnumerable<ErrorInfo> Errors { get; }
}
