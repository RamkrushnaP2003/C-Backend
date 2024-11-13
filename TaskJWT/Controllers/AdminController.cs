using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskJWT.Models;
using TaskJWT.Services.Implementations;
using TaskJWT.Services.Interfaces;

namespace TaskJWT.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserReadService _userReadService;
        private readonly IUserWriteService _userWriteService;

        public AdminController(IUserReadService userReadService, IUserWriteService userWriteService)
        {
            _userReadService = userReadService;
            _userWriteService = userWriteService;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userReadService.GetAllUsers());
        }

        [HttpGet("managers")]
        public IActionResult GetAllManagers()
        {
            return Ok(_userReadService.GetAllManagers());
        }

        [HttpGet("employees")]
        public IActionResult GetAllEmployees()
        {
            return Ok(_userReadService.GetAllEmployees());
        }

        [HttpPost("new-user")]
        public IActionResult CreateUser([FromBody] CreateUserModel newUser) 
        {
            _userWriteService.CreateUser(newUser);
            return Ok(new { message = "User created successfully." });
        }
    }
}
