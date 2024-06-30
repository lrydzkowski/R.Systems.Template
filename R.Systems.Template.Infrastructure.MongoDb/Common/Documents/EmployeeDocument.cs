using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Documents;

internal class EmployeeDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.Int64)]
    public long Id { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public long CompanyId { get; set; }
}
