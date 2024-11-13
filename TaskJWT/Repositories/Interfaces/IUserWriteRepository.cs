using TaskJWT.Models;

namespace TaskJWT.Repositories.Interfaces
{
    public interface IUserWriteRepository
    {
        CreateUserModel CreateUser(CreateUserModel newUser);

        User GetByUsername(string username);
    }
}