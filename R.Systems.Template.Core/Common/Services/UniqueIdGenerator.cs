namespace R.Systems.Template.Core.Common.Services;

public interface IUniqueIdGenerator
{
    long Generate();
}

internal class UniqueIdGenerator
    : IUniqueIdGenerator
{
    private static readonly object Lock = new();
    private int _counter;
    private long _lastTicks;

    public long Generate()
    {
        lock (Lock)
        {
            long ticks = DateTime.UtcNow.Ticks;
            if (ticks <= _lastTicks)
            {
                ticks = _lastTicks + 1;
            }

            _lastTicks = ticks;

            int count = Interlocked.Increment(ref _counter) & 0xFFFFF;
            if (count == 0)
            {
                _lastTicks++;
                ticks = _lastTicks;
            }

            return (ticks << 20) | (uint)count;
        }
    }
}
