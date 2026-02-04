using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmploymentManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime JoiningDate { get; set; }

        

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public Department Department { get; set; } = null!;

        [NotMapped]
        public string DepartmentName { get; set; } = null;

       
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
