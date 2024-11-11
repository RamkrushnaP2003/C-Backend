using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskJWT.Models;
using TaskJWT.Services.Interfaces;

namespace TaskJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IUserService _userService;
        public ManagerController(IUserService userService) {
            _userService = userService;
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet("data")]
        public IActionResult GetAllManager()
        {
            List<User> managers = _userService.GetAllManagers();
            return Ok(managers);
        }
    }
}
