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

        public CreateUserModel CreateUser(CreateUserModel newUser) {
            return _userWriteRepository.CreateUser(newUser);
        }
    }
}
