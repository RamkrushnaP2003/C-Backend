using TaskJWT.Models;
using TaskJWT.Repositories.Interfaces;
using BCrypt.Net;
using TaskJWT.Services.Interfaces;

namespace TaskJWT.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public void CreateUser(string username, string passwordHash, int roleId)
        {
            _userRepository.CreateUser(username, passwordHash, roleId);
        }

        public List<User> GetAllManagers()
        {
            return _userRepository.GetAllManagers();
        }

        public List<User> GetAllEmployees()
        {
            return _userRepository.GetAllEmployees();
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
    }
}
