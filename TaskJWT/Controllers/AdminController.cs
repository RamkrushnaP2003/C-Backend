using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskJWT.Models;
using TaskJWT.Services;

namespace TaskJWT.Controllers
{
    // Only authorized Admin users can access this controller
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // [AllowAnonymous]
        [HttpPost("create-user")]
        public IActionResult CreateUser([FromBody] CreateUserModel model)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            _userService.CreateUser(model.Username, passwordHash, (int)model.Role);
            return Ok("User created successfully.");
        }

        /*[AllowAnonymous] 
        [HttpPost("create-initial-admin")]
        public IActionResult CreateInitialAdmin([FromBody] CreateUserModel model)
        {
            // Hash the password for security
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            _userService.CreateUser(model.Username, passwordHash, 1); // RoleId for Admin
            return Ok("Initial Admin user created successfully.");
        }*/
    }
}
