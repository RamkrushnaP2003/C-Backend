using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskJWT.Models;
using TaskJWT.Services;

namespace TaskJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUserService _userService;

        public EmployeeController(IUserService userService) {
            _userService = userService;
        } 

        [HttpGet("data")]
        [Authorize(Roles = "Employee")]
        public IActionResult GetEmployeeData()
        {
            var employees = _userService.GetAllEmployees();
            return Ok(employees);
        }
    }
}
