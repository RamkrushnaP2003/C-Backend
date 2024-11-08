using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskJWT.Models;
using TaskJWT.Services;

namespace TaskJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // Endpoint for logging in
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = _userService.Authenticate(loginModel.Username, loginModel.Password);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            var token = _tokenService.GenerateToken(user); // Example: "Admin" role
            return Ok(new { Token = token });
        }
    }
}
