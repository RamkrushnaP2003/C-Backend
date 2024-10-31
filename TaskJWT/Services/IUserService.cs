using TaskJWT.Models;

namespace TaskJWT.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        void CreateUser(string username, string passwordHash, int roleId);
        List<User> GetAllManagers();
        List<User> GetAllEmployees();
    }
}
