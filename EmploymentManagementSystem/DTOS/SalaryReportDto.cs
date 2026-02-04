namespace EmploymentManagementSystem.DTOS
{
    public class SalaryReportDto
    {

        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime JoiningDate { get; set; }


    }
}
