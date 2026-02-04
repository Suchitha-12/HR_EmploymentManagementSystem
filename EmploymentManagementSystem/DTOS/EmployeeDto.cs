using System.ComponentModel.DataAnnotations;

namespace EmploymentManagementSystem.DTOS
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee code is required")] // ✅ Add this
        [StringLength(50)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        public DateTime JoiningDate { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        
        public string Position { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive value")]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
