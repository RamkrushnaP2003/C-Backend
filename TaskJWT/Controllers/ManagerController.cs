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
        private readonly IUserReadService _userReadService;
        public ManagerController(IUserReadService userReadService) {
            _userReadService = userReadService;
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet("data")]
        public IActionResult GetAllManager()
        {
            List<User> managers = _userReadService.GetAllManagers();
            return Ok(managers);
        }
    }
}
