namespace R.Systems.Template.Core.Common.Services;

public interface IUniqueIdGenerator
{
    long Generate();
}

internal class UniqueIdGenerator
    : IUniqueIdGenerator
{
    public long Generate()
    {
        byte[] buffer = Guid.NewGuid().ToByteArray();

        return BitConverter.ToInt64(buffer, 0);
    }
}
