using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskExcelMongoDB.Models;

namespace TaskExcelMongoDB.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();

        Task<User> CreateNewUser([FromBody] User newUser);

        Task<User> EditUser([FromBody] User updatedUser, string id);
    }
}