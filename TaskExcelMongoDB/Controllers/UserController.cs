using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskExcelMongoDB.Data;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Services.Implementations;
using TaskExcelMongoDB.Services.Interfaces;

namespace TaskExcelMongoDB.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MongoDBContext _mongoDbContext;
        private readonly IUserService _userService;
        public UserController(MongoDBContext mongoDBContext, IUserService userService) 
        {
            _mongoDbContext = mongoDBContext;
            _userService = userService;
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers() 
        {
            return await _userService.GetAllUsers();  
        }

        [HttpPost("create-new-user")]
        public async Task<IActionResult> CreateNewUser([FromBody] User newUser) 
        {
            return await _userService.CreateNewUser(newUser);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditUser([FromBody] User updatedUser,string id) 
        {
            return await _userService.EditUser(updatedUser, id);
        }
    }
}