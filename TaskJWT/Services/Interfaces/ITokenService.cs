using TaskJWT.Models;

namespace TaskJWT.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
