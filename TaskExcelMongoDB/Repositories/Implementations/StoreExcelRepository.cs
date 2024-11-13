using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskExcelMongoDB.Data;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Repositories.Interfaces;
using ExcelDataReader;

namespace TaskExcelMongoDB.Repositories.Implementations
{
    public class StoreExcelRepository : IStoreExcelRepository
    {
        private readonly MongoDBContext _mongoDbContext;

        public StoreExcelRepository(MongoDBContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task<bool> InsertUsers(List<User> users)
        {
            if (users == null || users.Count == 0) return false;

            await _mongoDbContext.Users.InsertManyAsync(users);
            return true;
        }
    }
}
