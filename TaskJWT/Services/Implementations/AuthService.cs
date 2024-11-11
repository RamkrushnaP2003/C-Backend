using TaskJWT.Models;
using TaskJWT.Repositories.Interfaces;
using TaskJWT.Services.Interfaces;
using BCrypt.Net;

namespace TaskJWT.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserWriteRepository _userWriteRepository;

        public AuthService(IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
        }

        public User Authenticate(string username, string password)
        {
            var user = _userWriteRepository.GetByUsername(username);
            return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
        }
    }
}
