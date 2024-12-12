using MongoDB.Driver;
using TaskExcelMongoDB.Data;
using TaskExcelMongoDB.Models;

namespace TaskExcelMongoDB.Tests.Data
{
    public class TestMongoDBContext : IMongoDBContext
    {
        private readonly IMongoCollection<User> _mockUserCollection;

        public TestMongoDBContext(IMongoCollection<User> mockUserCollection)
        {
            _mockUserCollection = mockUserCollection;
        }

        public IMongoCollection<User> Users => _mockUserCollection;
    }
}
