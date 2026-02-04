using System.ComponentModel.DataAnnotations;

namespace EmploymentManagementSystem.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentDescription { get; set; } = string.Empty;
        public string? DepartmentManagerName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
