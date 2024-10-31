using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskJWT.Models;
using TaskJWT.Services;

namespace TaskJWT.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IUserService _userService;
        public ManagerController(IUserService userService) {
            _userService = userService;
        }

        [HttpGet("data")]
        public IActionResult GetAllManager()
        {
            List<User> managers = _userService.GetAllManagers();
            return Ok(managers);
        }
    }
}
