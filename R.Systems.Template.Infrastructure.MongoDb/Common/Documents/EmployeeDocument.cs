using MongoDB.Bson.Serialization.Attributes;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Documents;

internal class EmployeeDocument
{
    [BsonId] public Guid Id { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public Guid CompanyId { get; set; }
}
