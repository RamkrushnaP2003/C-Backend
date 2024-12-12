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
            
            var users = await _userRepository.GetAllUsers();
            var response = new Dictionary<string, object>
            {
                { "Users", users }
            };

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> CreateNewUser([FromBody] User newUser) 
        {           
            // if(newUser==null) {
            //     return new BadRequestObjectResult(new { Message = "Fill the user deatils"});
            // }
            // await _userRepository.CreateNewUser(newUser);
            // return new OkObjectResult(new {Users = newUser});
            if (newUser == null)
            {
                return new BadRequestObjectResult(new Dictionary<string, object>
                {
                    { "Message", "Fill the user details" }
                });
            }

            // Simulate repository interaction
            await _userRepository.CreateNewUser(newUser);

            return new OkObjectResult(new Dictionary<string, object>
            {
                { "Message", newUser } // Returning the created user
            });
        }

        public async Task<IActionResult> EditUser([FromBody] User updatedUser, string id) 
        {
            if(updatedUser.FullName == string.Empty || updatedUser.Address == string.Empty || updatedUser.Address == string.Empty || updatedUser.DateOfBirth == string.Empty) {
                return new BadRequestObjectResult(new Dictionary<string, object>
                {
                    { "Message", "Enter valid information" }
                });
            }
            await _userRepository.EditUser(updatedUser, id);
            return new OkObjectResult(new Dictionary<string, object>
            {
                { "Message", $"User with id : {id} updated" }
            });
        }
    }
}