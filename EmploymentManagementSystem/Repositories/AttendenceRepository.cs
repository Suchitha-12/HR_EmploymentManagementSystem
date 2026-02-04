using EmploymentManagementSystem.Data;
using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentManagementSystem.Repositories
{
    public class AttendenceRepository : IAttendenceRepository
    {
        private readonly EmployeeDBContext _context;

        public AttendenceRepository(EmployeeDBContext context)
        {
            _context = context;
        }

        public async Task<AttendanceResponseDTO?> GetByAttendanceIdAsync(int attendanceId)
        {
            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .FirstOrDefaultAsync(a => a.AttendanceId == attendanceId);

            if (attendance == null)
                return null;

            return MapToResponseDto(attendance);
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> GetAllAttendanceAsync()
        {
            var attendances = await _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Employee.EmployeeCode)
                .ToListAsync();

            return attendances.Select(MapToResponseDto).ToList();
        }

        public async Task<AttendanceResponseDTO> AddAttendanceAsync(AttendanceDto attendanceDto)
        {
            // Calculate work hours if both check-in and check-out times are provided
            decimal workHours = attendanceDto.WorkHours;
            if (attendanceDto.CheckInTime.HasValue && attendanceDto.CheckOutTime.HasValue)
            {
                var totalHours = (attendanceDto.CheckOutTime.Value - attendanceDto.CheckInTime.Value).TotalHours;
                workHours = (decimal)totalHours;
            }

            var attendance = new Attendance
            {
                EmployeeId = attendanceDto.EmployeeId,
                Date = attendanceDto.Date,
                CheckInTime = attendanceDto.CheckInTime,
                CheckOutTime = attendanceDto.CheckOutTime,
                Status = attendanceDto.Status,
                WorkHours = workHours,
                Remarks = attendanceDto.Remarks,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync();

            // Reload with navigation properties
            await _context.Entry(attendance)
                .Reference(a => a.Employee)
                .LoadAsync();

            if (attendance.Employee != null)
            {
                await _context.Entry(attendance.Employee)
                    .Reference(e => e.Department)
                    .LoadAsync();
            }

            return MapToResponseDto(attendance);
        }

        public async Task<bool> UpdateAttendanceAsync(int attendanceId, AttendanceDto attendanceDto)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);

            if (attendance == null)
                return false;

            // Calculate work hours if both check-in and check-out times are provided
            decimal workHours = attendanceDto.WorkHours;
            if (attendanceDto.CheckInTime.HasValue && attendanceDto.CheckOutTime.HasValue)
            {
                var totalHours = (attendanceDto.CheckOutTime.Value - attendanceDto.CheckInTime.Value).TotalHours;
                workHours = (decimal)totalHours;
            }

            attendance.EmployeeId = attendanceDto.EmployeeId;
            attendance.Date = attendanceDto.Date;
            attendance.CheckInTime = attendanceDto.CheckInTime;
            attendance.CheckOutTime = attendanceDto.CheckOutTime;
            attendance.Status = attendanceDto.Status;
            attendance.WorkHours = workHours;
            attendance.Remarks = attendanceDto.Remarks;

            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAttendanceAsync(int attendanceId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);

            if (attendance == null)
                return false;

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return true;
        }

        // Helper method to map Attendance model to AttendanceResponseDTO
        private static AttendanceResponseDTO MapToResponseDto(Attendance attendance)
        {
            return new AttendanceResponseDTO
            {
                AttendanceId = attendance.AttendanceId,
                EmployeeId = attendance.EmployeeId,
                EmployeeCode = attendance.Employee?.EmployeeCode ?? "",
                EmployeeName = attendance.Employee != null
                    ? $"{attendance.Employee.FirstName} {attendance.Employee.LastName}"
                    : "",
                DepartmentName = attendance.Employee?.Department?.DepartmentName ?? "",
                Date = attendance.Date,
                CheckInTime = attendance.CheckInTime?.ToString(@"hh\:mm\:ss"),
                CheckOutTime = attendance.CheckOutTime?.ToString(@"hh\:mm\:ss"),
                Status = attendance.Status,
                WorkHours = attendance.WorkHours,
                Remarks = attendance.Remarks,
                CreatedAt = attendance.CreatedAt
            };
        }
    }
}
