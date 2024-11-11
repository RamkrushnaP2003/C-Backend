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

        public AdminController(IUserReadService userReadService)
        {
            _userReadService = userReadService;
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
    }
}
