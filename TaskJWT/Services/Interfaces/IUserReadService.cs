using TaskJWT.Models;
using System.Collections.Generic;

namespace TaskJWT.Services.Interfaces
{
    public interface IUserReadService
    {
        List<User> GetAllManagers();
        List<User> GetAllEmployees();
        List<User> GetAllUsers();
    }
}
