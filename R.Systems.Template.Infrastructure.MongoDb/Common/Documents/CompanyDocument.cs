using MongoDB.Bson.Serialization.Attributes;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Documents;

internal class CompanyDocument
{
    [BsonId] public Guid Id { get; set; }

    public string Name { get; set; } = "";
}
