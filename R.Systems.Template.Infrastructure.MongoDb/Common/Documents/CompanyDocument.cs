using MongoDB.Bson;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Documents;

internal class CompanyDocument
{
    public ObjectId Id { get; set; }

    public string Name { get; set; } = "";
}
