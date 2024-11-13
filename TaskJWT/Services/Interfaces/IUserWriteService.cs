using TaskJWT.Models;
using System.Collections.Generic;

namespace TaskJWT.Services.Interfaces
{
    public interface IUserWriteService
    {
        CreateUserModel CreateUser(CreateUserModel newUser);
    }
}
