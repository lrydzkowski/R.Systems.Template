using MongoDB.Bson;

namespace R.Systems.Template.Infrastructure.MongoDb.Common.Documents;

internal class EmployeeDocument
{
    public ObjectId Id { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public CompanyDocument Company { get; set; } = new();
}
