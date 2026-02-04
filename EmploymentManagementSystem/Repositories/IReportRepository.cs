using EmploymentManagementSystem.DTOS;

namespace EmploymentManagementSystem.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<EmployeeReportDto>> GetEmployeeDirectoryAsync();
        Task<IEnumerable<DepartmentReportDto>> GetDepartmentReportAsync();
        Task<IEnumerable<AttendanceResponseDTO>> GetAttendanceReportAsync(DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<SalaryReportDto>> GetSalaryReportAsync();

    }
}
