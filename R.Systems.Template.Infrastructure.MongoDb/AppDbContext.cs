using Microsoft.Extensions.Options;
using MongoDB.Driver;
using R.Systems.Template.Infrastructure.MongoDb.Common;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;
using R.Systems.Template.Infrastructure.MongoDb.Common.Options;

namespace R.Systems.Template.Infrastructure.MongoDb;

internal class AppDbContext
{
    public AppDbContext(IOptions<ConnectionStringsOptions> options)
    {
        MongoUrl mongoUrl = new(options.Value.MongoDb);
        MongoClient client = new(mongoUrl);
        IMongoDatabase database = client.GetDatabase(mongoUrl.DatabaseName);

        Companies = database.GetCollection<CompanyDocument>(Consts.Collections.Companies);
        Employees = database.GetCollection<EmployeeDocument>(Consts.Collections.Employees);
    }

    public IMongoCollection<CompanyDocument> Companies { get; }
    public IMongoCollection<EmployeeDocument> Employees { get; }
}
