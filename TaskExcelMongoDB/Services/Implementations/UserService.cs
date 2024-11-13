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
            try 
            {
                List<User> users = await _userRepository.GetAllUsers();

                if (users == null || users.Count == 0) 
                {
                    return new BadRequestObjectResult(new { Message = "No users found. Please add users." });
                }

                return new OkObjectResult(new { Users = users });
            } 
            catch (Exception ex) 
            {
                return new ObjectResult(new { Message = $"Internal server error: {ex.Message}" }) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> CreateNewUser([FromBody] User newUser) 
        {
            try {
                if(newUser==null) {
                    return new BadRequestObjectResult(new { Message = "Fill the user deatils"});
                }
                await _userRepository.CreateNewUser(newUser);
                return new OkObjectResult(new {Users = newUser});
            } catch (Exception ex) {
                return new ObjectResult(new {Message = $"Internal server err : {ex.Message}"}) {StatusCode = 500};
            }
        }

        public async Task<IActionResult> EditUser([FromBody] User updatedUser, string id) 
        {
            try {
                if(updatedUser.FullName == string.Empty || updatedUser.Address == string.Empty || updatedUser.Address == string.Empty || updatedUser.DateOfBirth == string.Empty) return new BadRequestObjectResult(new {Message = "Enter valid information"}); 
                await _userRepository.EditUser(updatedUser, id);
                return new OkObjectResult(new {Message = $"User with id : {id} updated"});
            } catch (Exception ex) {
                return new BadRequestObjectResult(new {Message = $"Internal server errr : {ex.Message}"}) {StatusCode = 500};
            }
        }
    }
}