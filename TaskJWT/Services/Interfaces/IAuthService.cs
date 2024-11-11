using TaskJWT.Models;
using System.Collections.Generic;

namespace TaskJWT.Services.Interfaces
{
    public interface IAuthService
    {
        User Authenticate(string username, string password);
    }
}
