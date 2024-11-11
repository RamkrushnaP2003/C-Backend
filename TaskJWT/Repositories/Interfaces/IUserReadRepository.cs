using TaskJWT.Models;
using System.Collections.Generic;

namespace TaskJWT.Repositories.Interfaces
{
    public interface IUserReadRepository
    {
        List<User> GetAllUsers();
        List<User> GetAllManagers();
        List<User> GetAllEmployees();
        
    }
}
