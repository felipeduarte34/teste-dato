using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TesteBackend.Common.General;

public class MongoDbContext
{
    private readonly IMongoDatabase _database = null;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        if (client != null)
            _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}