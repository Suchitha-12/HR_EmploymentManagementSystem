namespace EmploymentManagementSystem.DTOS
{
    public class DepartMentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentDescription { get; set; } = string.Empty;
        public string? DepartmentManagerName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
