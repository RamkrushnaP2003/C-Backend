using Microsoft.AspNetCore.Mvc;
using TaskJWT.Models;
using TaskJWT.Services;
using TaskJWT.Services.Interfaces;

namespace TaskJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
                return Unauthorized();

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
