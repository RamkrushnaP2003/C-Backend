using TaskJWT.Models;
using TaskJWT.Repositories.Interfaces;
using BCrypt.Net;
using TaskJWT.Services.Interfaces;

namespace TaskJWT.Services.Implementations
{
    public class UserWriteService : IUserWriteService
    {
        private readonly IUserWriteRepository _userWriteRepository;

        public UserWriteService(IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
        }

        public void CreateUser(string username, string passwordHash, int roleId) => _userWriteRepository.CreateUser(username, passwordHash, roleId);

    }
}
