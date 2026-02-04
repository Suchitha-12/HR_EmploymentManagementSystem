using EmploymentManagementSystem.DTOS;

namespace EmploymentManagementSystem.Repositories
{
    public interface IAttendenceRepository
    {
        Task<AttendanceResponseDTO?> GetByAttendanceIdAsync(int attendanceId);
        Task<IEnumerable<AttendanceResponseDTO>> GetAllAttendanceAsync();
        Task<AttendanceResponseDTO> AddAttendanceAsync(AttendanceDto attendanceDto);
        Task<bool> UpdateAttendanceAsync(int attendanceId, AttendanceDto attendanceDto);
        Task<bool> DeleteAttendanceAsync(int attendanceId);
    }
}
