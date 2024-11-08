namespace TaskJWT.Models
{
    public class CreateUserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } 
    }
}
