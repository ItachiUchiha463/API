using MongoDB.Driver;

namespace SortTest.Test;

public class MobgoDBClient
{
    private static IMongoDatabase _db;
    private static MobgoDBClient _instance;

    public static MobgoDBClient Instance
    {
        get => _instance ?? new MobgoDBClient();
    }

    private MobgoDBClient()
    {
        var connectionString = "mongodb+srv://rostikkovalchuk856_db_user:5jByDdLky6dXjsPi@databasekovalchuk.yen50ht.mongodb.net/?appName=DataBaseKovalchuk&tlsInsecure=true"; 
        var client = new MongoClient(connectionString);
        _db = client.GetDatabase("mydatabase");

    }

    public IMongoCollection<T> GetCollection<T>(string name) => _db.GetCollection<T>(name);
}