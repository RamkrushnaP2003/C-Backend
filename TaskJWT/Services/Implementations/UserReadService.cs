using TaskJWT.Models;
using TaskJWT.Repositories.Interfaces;
using BCrypt.Net;
using TaskJWT.Services.Interfaces;

namespace TaskJWT.Services.Implementations
{
    public class UserReadService : IUserReadService
    {
        private readonly IUserReadRepository _userReadRepository;

        public UserReadService(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }
 
        public List<User> GetAllManagers() => _userReadRepository.GetAllManagers();
 
        public List<User> GetAllEmployees() => _userReadRepository.GetAllEmployees();

        public List<User> GetAllUsers() =>_userReadRepository.GetAllUsers();
    }
}
