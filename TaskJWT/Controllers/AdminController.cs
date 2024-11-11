using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskJWT.Models;
using TaskJWT.Services.Interfaces;

namespace TaskJWT.Controllers
{
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

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("managers")]
        public IActionResult GetAllManagers()
        {
            return Ok(_userService.GetAllManagers());
        }

        [HttpGet("employees")]
        public IActionResult GetAllEmployees()
        {
            return Ok(_userService.GetAllEmployees());
        }
    }
}
