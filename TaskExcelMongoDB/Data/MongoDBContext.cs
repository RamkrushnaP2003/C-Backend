using MongoDB.Driver;
using TaskExcelMongoDB.Models;

namespace TaskExcelMongoDB.Data
{
    public interface IMongoDBContext
    {
        IMongoCollection<User> Users { get; }
    }

    public class MongoDBContext : IMongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            _database = new MongoClient(configuration.GetConnectionString("MongoDB")).GetDatabase(configuration["MongoDatabase"]);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    }

    
}
