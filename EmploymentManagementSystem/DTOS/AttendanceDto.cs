using System.ComponentModel.DataAnnotations;

namespace EmploymentManagementSystem.DTOS
{
    public class AttendanceDto
    {
      

        [Required(ErrorMessage = "Employee ID is required")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        public TimeSpan? CheckInTime { get; set; }

        public TimeSpan? CheckOutTime { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; } = string.Empty;

        [Range(0, 24, ErrorMessage = "Work hours must be between 0 and 24")]
        public decimal WorkHours { get; set; }

        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
        public string? Remarks { get; set; }

       
    }
}
