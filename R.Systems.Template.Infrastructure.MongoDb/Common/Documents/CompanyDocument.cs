using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Documents;

internal class CompanyDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.Int64)]
    public long Id { get; set; }

    public string Name { get; set; } = "";
}
