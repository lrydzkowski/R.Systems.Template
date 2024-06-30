namespace R.Systems.Template.Infrastructure.MongoDb.Common.Services;

internal interface IUniqueIdGenerator
{
    public long Generate();
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
