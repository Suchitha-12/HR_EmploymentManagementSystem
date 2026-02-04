
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmploymentManagementSystem.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public string Status { get; set; }
        public decimal WorkHours { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Employee Employee { get; set; } = null!;

        public static class AttendanceStatus
        {
            public const string Present = "Present";
            public const string Absent = "Absent";
            public const string Leave = "Leave";
            public const string HalfDay = "HalfDay";
            public const string Weekend = "Weekend";
            public const string Holiday = "Holiday";
        }


    }
}
