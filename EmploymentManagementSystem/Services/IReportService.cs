namespace EmploymentManagementSystem.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateEmployeeDirectoryPdfAsync();
        Task<byte[]> GenerateEmployeeDirectoryExcelAsync();
        Task<byte[]> GenerateDepartmentReportPdfAsync();
        Task<byte[]> GenerateDepartmentReportExcelAsync();
        Task<byte[]> GenerateAttendanceReportPdfAsync(DateTime? startDate, DateTime? endDate);
        Task<byte[]> GenerateAttendanceReportExcelAsync(DateTime? startDate, DateTime? endDate);
        Task<byte[]> GenerateSalaryReportPdfAsync();
        Task<byte[]> GenerateSalaryReportExcelAsync();
    }
}
