using MongoDB.Driver;
using TaskExcelMongoDB.Models;


namespace TaskExcelMongoDB.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration) 
        {
            _database = new MongoClient(configuration.GetConnectionString("MongoDB")).GetDatabase(configuration["MongoDatabase"]);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    }
}