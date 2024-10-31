namespace TaskJWT.Models
{
    public class User
    {
        public int Id { get; set; } // Unique identifier for the user
        public string Username { get; set; } // Username of the user
        public string PasswordHash { get; set; } // Hashed password
        public UserRole Role { get; set; } // Foreign key to Role
        public string RoleName => Role.ToString(); // find the rolename which is equivalent to role in enum

        // Navigation property (optional)
        // public Role RoleEntity { get; set; } // Reference to the Role model
    }
}
