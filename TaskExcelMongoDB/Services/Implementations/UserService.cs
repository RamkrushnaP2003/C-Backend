using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Repositories.Interfaces;
using TaskExcelMongoDB.Services.Interfaces;

namespace TaskExcelMongoDB.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> GetAllUsers() 
        {
            
            List<User> users = await _userRepository.GetAllUsers();
            if (users == null || users.Count == 0) 
            {
                return new BadRequestObjectResult(new { Message = "No users found. Please add users." });
            }
            return new OkObjectResult(new { Users = users });
        }

        public async Task<IActionResult> CreateNewUser([FromBody] User newUser) 
        {           
            if(newUser==null) {
                return new BadRequestObjectResult(new { Message = "Fill the user deatils"});
            }
            await _userRepository.CreateNewUser(newUser);
            return new OkObjectResult(new {Users = newUser});
        }

        public async Task<IActionResult> EditUser([FromBody] User updatedUser, string id) 
        {
            if(updatedUser.FullName == string.Empty || updatedUser.Address == string.Empty || updatedUser.Address == string.Empty || updatedUser.DateOfBirth == string.Empty) return new BadRequestObjectResult(new {Message = "Enter valid information"}); 
            await _userRepository.EditUser(updatedUser, id);
            return new OkObjectResult(new {Message = $"User with id : {id} updated"});
        }
    }
}