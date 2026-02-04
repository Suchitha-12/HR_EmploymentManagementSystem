namespace EmploymentManagementSystem.DTOS
{
    public class AttendanceResponseDTO
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal WorkHours { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
