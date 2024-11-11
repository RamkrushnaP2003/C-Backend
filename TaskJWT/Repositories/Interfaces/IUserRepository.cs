using TaskJWT.Models;
using System.Collections.Generic;

namespace TaskJWT.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        void CreateUser(string username, string passwordHash, int roleId);
        List<User> GetAllUsers();
        List<User> GetAllManagers();
        List<User> GetAllEmployees();
    }
}
