namespace EmploymentManagementSystem.DTOS
{
    public class EmployeeReportDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Status { get; set; } = string.Empty;

    }
}
