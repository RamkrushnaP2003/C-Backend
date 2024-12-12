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
        private readonly IMongoDBContext _mongoDbContext;

        public StoreExcelRepository(IMongoDBContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task<bool> InsertUsers(List<User> users)
        {
            if (users == null || users.Count == 0)
                throw new ArgumentException("Users list cannot be null or empty.");

            if (users.Any(u => string.IsNullOrEmpty(u.FullName) || string.IsNullOrEmpty(u.Address) || string.IsNullOrEmpty(u.MobileNo)))
                throw new ArgumentException("User data contains empty or null fields.");

            await _mongoDbContext.Users.InsertManyAsync(users);
            return true;
        }
    }
}
