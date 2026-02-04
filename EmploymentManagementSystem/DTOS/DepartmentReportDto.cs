namespace EmploymentManagementSystem.DTOS
{
    public class DepartmentReportDto
    {

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public int TotalEmployees { get; set; }
        public string Status { get; set; } = string.Empty;

    }
}
