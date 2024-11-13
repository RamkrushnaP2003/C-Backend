using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskExcelMongoDB.Models;

namespace TaskExcelMongoDB.Services.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> GetAllUsers();

        Task<IActionResult> CreateNewUser([FromBody] User newUser);

        Task<IActionResult> EditUser([FromBody] User updatedUser, string id);
    }
}