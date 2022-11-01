namespace R.Systems.Template.Core.Common.Validation;

public readonly struct Result<TData>
{
    public Result(TData? value)
    {
        State = ResultState.Success;
        Value = value;
        Exception = null;
    }

    public Result(Exception? e)
    {
        State = ResultState.Faulted;
        Value = default;
        Exception = e;
    }

    public static implicit operator Result<TData>(TData value) => new(value);

    public bool IsFaulted => State == ResultState.Faulted;

    public bool IsSuccess => State == ResultState.Success;

    public TData? Value { get; }

    private ResultState State { get; }

    private Exception? Exception { get; }

    public TReturn Match<TReturn>(Func<TData?, TReturn> success, Func<Exception?, TReturn> fail) =>
        IsFaulted ? fail(Exception) : success(Value);

    public Result<TNewData> Map<TNewData>(Func<TData?, TNewData> f) =>
        IsFaulted ? new Result<TNewData>(Exception) : new Result<TNewData>(f(Value));

    public Result<TNewData> MapFaulted<TNewData>() => new(Exception);

    public async Task<Result<TNewData>> MapAsync<TNewData>(Func<TData?, Task<TNewData>> f) =>
        IsFaulted ? new Result<TNewData>(Exception) : new Result<TNewData>(await f(Value));
}
