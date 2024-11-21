using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskExcelMongoDB.Data;
using TaskExcelMongoDB.Exceptions;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Repositories.Interfaces;

namespace TaskExcelMongoDB.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDBContext _mongoDBContext;

        public UserRepository(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _mongoDBContext.Users.FindAsync(Builders<User>.Filter.Empty);

            Console.WriteLine($"Mongo Cursor : {users}");

            List<User> userList = await users.ToListAsync();

            Console.WriteLine($"Retrieved used count = ${userList.Count}");

            if (userList == null || userList.Any(user => string.IsNullOrEmpty(user.FullName) || user.DateOfBirth == null))
            {
                throw new InvalidUserDataException("User data contains null or empty fields.");
            }

            return userList;
        }

        public async Task<User> CreateNewUser([FromBody] User newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.FullName) || string.IsNullOrEmpty(newUser.Address) ||
                string.IsNullOrEmpty(newUser.DateOfBirth) || string.IsNullOrEmpty(newUser.MobileNo))
            {
                throw new InvalidUserDataException("User data contains null or empty fields.");
            }

            await _mongoDBContext.Users.InsertOneAsync(newUser);
            return newUser;
        }

        public async Task<User> EditUser(User updatedUser, string id)
        {
            if (updatedUser == null || string.IsNullOrEmpty(updatedUser.FullName) || string.IsNullOrEmpty(updatedUser.Address) ||
                string.IsNullOrEmpty(updatedUser.DateOfBirth) || string.IsNullOrEmpty(updatedUser.MobileNo))
            {
                throw new InvalidUserDataException("User data contains null or empty fields.");
            }

            var filter = Builders<User>.Filter.Eq(user => user.Id, id);
            var update = Builders<User>.Update
                .Set(u => u.FullName, updatedUser.FullName)
                .Set(u => u.MobileNo, updatedUser.MobileNo)
                .Set(u => u.Address, updatedUser.Address)
                .Set(u => u.Salary, updatedUser.Salary)
                .Set(u => u.DateOfBirth, updatedUser.DateOfBirth);

            var result = await _mongoDBContext.Users.FindOneAndUpdateAsync(filter, update);
            return result ?? throw new InvalidOperationException("Update failed; user not found.");
        }
    }
}
