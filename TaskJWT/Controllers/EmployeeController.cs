using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskJWT.Models;
using TaskJWT.Services.Interfaces;

namespace TaskJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUserReadService _userReadService;

        public EmployeeController(IUserReadService userReadService) 
        {
            _userReadService = userReadService;
        } 

        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("data")]
        public IActionResult GetEmployeeData()
        {
            var employees = _userReadService.GetAllEmployees();
            return Ok(employees);
        }
    }
}
