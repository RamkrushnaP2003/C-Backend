using TaskJWT.Models;
using System.Collections.Generic;

namespace TaskJWT.Services.Interfaces
{
    public interface IUserWriteService
    {
        void CreateUser(string username, string passwordHash, int roleId);
    }
}
