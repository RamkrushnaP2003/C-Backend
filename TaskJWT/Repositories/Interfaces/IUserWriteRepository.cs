using TaskJWT.Models;

namespace TaskJWT.Repositories.Interfaces
{
    public interface IUserWriteRepository
    {
        void CreateUser(string username, string password, int roleId);

        User GetByUsername(string username);
    }
}