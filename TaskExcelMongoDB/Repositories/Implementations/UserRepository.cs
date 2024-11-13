using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskExcelMongoDB.Data;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Repositories.Interfaces;

namespace TaskExcelMongoDB.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoDBContext _mongoDBContext;

        public UserRepository(MongoDBContext mongoDBContext) {
            _mongoDBContext = mongoDBContext;
        }
        public async Task<List<User>> GetAllUsers()
        {
            try {
                List<User> users = new List<User>();
                users = await _mongoDBContext.Users.Find(Builders<User>.Filter.Where(user => true)).ToListAsync();
                Console.WriteLine(users.Count);
                return users;

            } catch (Exception ex) {
                throw new Exception($"Error fetching users: {ex.Message}");
            }
        }

        public async Task<User> CreateNewUser([FromBody] User newUser) 
        {
            try {
                await _mongoDBContext.Users.InsertOneAsync(newUser);
                Console.WriteLine("New user created.");
                return newUser;
            } catch (Exception ex) {
                throw new Exception($"Error while creating new user : {ex.Message}");
            }
        }

        public async Task<User> EditUser([FromBody] User updatedUser, string id)
        {
            try {
                var filter = Builders<User>.Filter.Eq(user => user.Id, id);
                var update = Builders<User>.Update
                    .Set(u => u.FullName, updatedUser.FullName)
                    .Set(u => u.MobileNo, updatedUser.MobileNo)
                    .Set(u => u.Address, updatedUser.Address)
                    .Set(u => u.Salary, updatedUser.Salary)
                    .Set(u => u.DateOfBirth, updatedUser.DateOfBirth);
                    var res = await _mongoDBContext.Users.FindOneAndUpdateAsync(filter, update);
                return updatedUser;
            } catch(Exception ex) {
                throw new Exception($"Error while updating user : {ex.Message}");
            }
        }
    }
}