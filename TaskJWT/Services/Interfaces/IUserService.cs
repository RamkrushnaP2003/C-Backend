using TaskJWT.Models;
using System.Collections.Generic;

namespace TaskJWT.Services.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        void CreateUser(string username, string passwordHash, int roleId);
        List<User> GetAllManagers();
        List<User> GetAllEmployees();
        List<User> GetAllUsers();
    }
}
