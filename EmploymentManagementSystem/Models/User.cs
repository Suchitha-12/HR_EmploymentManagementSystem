
using System.ComponentModel.DataAnnotations;

namespace EmploymentManagementSystem.Models
{
    public class User
    {

        [Key]

        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string User = "User";
        }




    }
}
